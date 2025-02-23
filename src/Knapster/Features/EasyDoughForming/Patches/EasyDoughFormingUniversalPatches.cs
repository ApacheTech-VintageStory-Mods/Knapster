using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Extensions;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;
using ArtOfCooking.BlockEntities;
using ArtOfCooking.Blocks;
using ArtOfCooking.Items;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Patches;

[HarmonyUniversalPatch]
[RequiresMod("coreofarts")]
[RequiresMod("artofcooking")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyDoughFormingUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockEntityDoughForm), "OnUseOver", typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool))]
    public static bool UniversalPatch_BlockEntityDoughForm_OnUseOver_Prefix(BlockEntityDoughForm __instance,
        IPlayer byPlayer, bool mouseBreakMode, Vec3i voxelPos, BlockFacing facing, ref ItemStack ___workItemStack)
    {
        var voxelsPerClick = ApiEx.Return(
            _ => EasyDoughFormingClient.Settings.VoxelsPerClick,
            _ => EasyDoughFormingServer.Settings.VoxelsPerClick);

        var instantComplete = ApiEx.Return(
            _ => EasyDoughFormingClient.Settings.InstantComplete,
            _ => EasyDoughFormingServer.Settings.InstantComplete);

        var enabled = ApiEx.Return(
            _ => EasyDoughFormingClient.Settings.Enabled,
            _ => EasyDoughFormingServer.IsEnabledFor(byPlayer));

        try
        {
            var slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack is null || !__instance.CanWorkCurrent) return true;
            if (slot.Itemstack.Collectible is not AOCItemDough dough) return true;
            var blockSel = new BlockSelection { Position = __instance.Pos };
            var toolMode = dough.GetToolMode(slot, byPlayer, blockSel);

            if (!enabled)
            {
                if (toolMode > 3) dough.SetToolMode(slot, byPlayer, blockSel, 0);
                return true;
            }

            if (toolMode < 4) return true;
            if (mouseBreakMode) return false;

            if (__instance.Api.Side.IsClient())
            {
                __instance.SendUseOverPacket(byPlayer, voxelPos, facing, false);
            }

            dough.SetToolMode(slot, byPlayer, blockSel, 0);

            var currentLayer = __instance.CurrentLayer();
            if (instantComplete
                    ? __instance.CompleteInTurn(slot)
                    : __instance.AutoCompleteLayer(currentLayer, voxelsPerClick))
            {
                __instance.Api.World.PlaySoundAt(new AssetLocation("sounds/player/clayform.ogg"), byPlayer, byPlayer, true, 8f);
            }
            __instance.Api.World.FrameProfiler.Mark("doughform-modified");
            currentLayer = __instance.CurrentLayer();
            __instance.CallMethod("RegenMeshAndSelectionBoxes", currentLayer);
            __instance.Api.World.FrameProfiler.Mark("doughform-regenmesh");
            __instance.Api.World.BlockAccessor.MarkBlockDirty(__instance.Pos);
            __instance.Api.World.BlockAccessor.MarkBlockEntityDirty(__instance.Pos);

            if (!__instance.CallMethod<bool>("HasAnyVoxel"))
            {
                __instance.AvailableVoxels = 0;
                ___workItemStack = null;
                __instance.Api.World.BlockAccessor.SetBlock(0, __instance.Pos);
                return false;
            }
            CheckIfFinished(__instance, byPlayer, currentLayer);
            __instance.Api.World.FrameProfiler.Mark("doughform-checkfinished");
            __instance.MarkDirty();

            if (slot.Itemstack is null) return false;
            dough.SetToolMode(slot, byPlayer, blockSel, 4);
            return false;
        }
        catch (ArgumentNullException ex)
        {
            ModEx.Mod.Logger.Error(ex);
            return true;
        }
    }

    public static void CheckIfFinished(BlockEntityDoughForm __instance, IPlayer byPlayer, int layer)
    {
        if (__instance.CallMethod<bool>("MatchesRecipe", layer) && __instance.Api.World is IServerWorldAccessor)
        {
            __instance.SetField("workItemStack", null);
            __instance.Voxels = new bool[16, 16, 16];
            __instance.AvailableVoxels = 0;
            var outstack = __instance.SelectedRecipe.Output.ResolvedItemstack.Clone();
            __instance.SetField("selectedRecipeId", -1);
            __instance.SetField("selectedRecipe", null);

            if (outstack.StackSize == 1 && outstack.Class == EnumItemClass.Block)
            {
                if ((outstack?.Collectible.FirstCodePart(0)) == "fakepie")
                {
                    var blockform = __instance.Api.World.GetBlock(new AssetLocation("pie-raw")) as BlockPie;
                    __instance.Api.World.BlockAccessor.SetBlock(blockform.BlockId, __instance.Pos);
                    var blockEntityPie = __instance.Api.World.BlockAccessor.GetBlockEntity(__instance.Pos) as BlockEntityPie;
                    byPlayer.InventoryManager.TryGiveItemstack(outstack, false);
                    blockEntityPie.OnPlaced(byPlayer);
                    return;
                }
                __instance.Api.World.BlockAccessor.SetBlock(outstack.Block.BlockId, __instance.Pos);
                return;
            }
            else
            {
                int tries = 500;
                while (outstack.StackSize > 0 && tries-- > 0)
                {
                    if ((outstack?.Collectible.FirstCodePart(0)) == "lavash")
                    {
                        var blockform2 = __instance.Api.World.GetBlock(new AssetLocation("artofcooking:shawarma-raw")) as AOCBlockShawarma;
                        __instance.Api.World.BlockAccessor.SetBlock(blockform2.BlockId, __instance.Pos);
                        (__instance.Api.World.BlockAccessor.GetBlockEntity(__instance.Pos) as AOCBEShawarma).OnFormed(byPlayer);
                        return;
                    }
                    var dropStack = outstack.Clone();
                    dropStack.StackSize = Math.Min(outstack.StackSize, outstack.Collectible.MaxStackSize);
                    outstack.StackSize -= dropStack.StackSize;
                    var tree = new TreeAttribute();
                    tree["itemstack"] = new ItemstackAttribute(dropStack);
                    tree["byentityid"] = new LongAttribute(byPlayer.Entity.EntityId);
                    __instance.Api.Event.PushEvent("onitemdoughformed", tree);
                    if (byPlayer.InventoryManager.TryGiveItemstack(dropStack, false))
                    {
                        __instance.Api.World.PlaySoundAt(new AssetLocation("sounds/player/collect"), byPlayer, null, true, 32f, 1f);
                    }
                    else
                    {
                        __instance.Api.World.SpawnItemEntity(dropStack, __instance.Pos, null);
                    }
                }
                if (tries <= 1)
                {
                    var logger = __instance.Api.World.Logger;
                    string str = "Tried to drop finished dough forming item but failed after 500 times?! Gave up doing so. Out stack was ";
                    var itemStack = outstack;
                    logger.Error(str + itemStack?.ToString());
                }
                __instance.Api.World.BlockAccessor.SetBlock(0, __instance.Pos);
            }
        }
    }
}