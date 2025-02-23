using ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Extensions;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Patches;

[HarmonyUniversalPatch]
[RequiresMod("coreofarts")]
[RequiresMod("artofcooking")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyDoughFormingUniversalPatches
{
    private const string BlockEntityDoughForm = "ArtOfCooking.BlockEntities.BlockEntityDoughForm";
    private const string AOCItemDough = "ArtOfCooking.Items.AOCItemDough";

    [HarmonyPrefix]
    [HarmonyPatchEx(BlockEntityDoughForm, "OnUseOver", typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool))]
    public static bool UniversalPatch_BlockEntityDoughForm_OnUseOver_Prefix(BlockEntity __instance,
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
            if (___workItemStack is null) return true;
            if (!__instance.CallMethod<bool>("CanWork", ___workItemStack)) return true;
            var slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack is null) return true;
            if (slot.Itemstack.Collectible.GetType().FullName != AOCItemDough) return true;
            var dough = slot.Itemstack.Collectible;
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
                __instance.CallMethod("SendUseOverPacket", byPlayer, voxelPos, facing, false);
            }

            dough.SetToolMode(slot, byPlayer, blockSel, 0);

            var currentLayer = __instance.CallMethod<int>("NextNotMatchingRecipeLayer", 0);
            if (instantComplete
                    ? __instance.CompleteInTurn(slot, 36)
                    : __instance.AutoCompleteLayer(currentLayer, voxelsPerClick))
            {
                __instance.Api.World.PlaySoundAt(new AssetLocation("sounds/player/clayform.ogg"), byPlayer, byPlayer, true, 8f);
            }
            __instance.Api.World.FrameProfiler.Mark("doughform-modified3");
            currentLayer = __instance.CallMethod<int>("NextNotMatchingRecipeLayer", currentLayer);
            __instance.CallMethod("RegenMeshAndSelectionBoxes", currentLayer);
            __instance.Api.World.FrameProfiler.Mark("doughform-regenmesh");
            __instance.Api.World.BlockAccessor.MarkBlockDirty(__instance.Pos);
            __instance.Api.World.BlockAccessor.MarkBlockEntityDirty(__instance.Pos);

            if (!__instance.CallMethod<bool>("HasAnyVoxel"))
            {
                __instance.SetField("AvailableVoxels", 0);
                ___workItemStack = null;
                __instance.Api.World.BlockAccessor.SetBlock(0, __instance.Pos);
                return false;
            }
            __instance.CallMethod("CheckIfFinished", byPlayer, currentLayer);
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
}