﻿using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Extensions;

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
    [HarmonyPatchEx("ArtOfCooking.BlockEntities.BlockEntityDoughForm", "OnUseOver", typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool))]
    public static bool UniversalPatch_BlockEntityDoughForm_OnUseOver_Prefix(dynamic __instance,
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
            if (slot.Itemstack?.Collectible.GetType().Name != "AOCItemDough") return true;
            var dough = slot.Itemstack.Collectible.To<Item>();

            var blockSel = new BlockSelection { Position = __instance.Pos };
            var toolMode = dough.GetToolMode(slot, byPlayer, blockSel);

            if (!enabled)
            {
                if (toolMode > 3) dough.SetToolMode(slot, byPlayer, blockSel, 0);
                return true;
            }

            if (toolMode < 4) return true;
            if (mouseBreakMode) return false;

            if (__instance.Api.Side == EnumAppSide.Client)
            {
                __instance.SendUseOverPacket(byPlayer, voxelPos, facing, false);
            }

            dough.SetToolMode(slot, byPlayer, blockSel, 0);
            var world = ApiEx.Current.World;
            int currentLayer = BlockEntityDoughFormExtensions.CurrentLayer(__instance);
            if (instantComplete
                    ? BlockEntityDoughFormExtensions.CompleteInTurn(__instance, slot)
                    : BlockEntityDoughFormExtensions.AutoCompleteLayer(__instance, currentLayer, voxelsPerClick))
            {
                world.PlaySoundAt(new AssetLocation("sounds/player/clayform.ogg"), byPlayer, byPlayer, true, 8f);
            }
            world.FrameProfiler.Mark("doughform-modified");
            currentLayer = BlockEntityDoughFormExtensions.CurrentLayer(__instance);
            HarmonyReflectionExtensions.CallMethod(__instance, "RegenMeshAndSelectionBoxes", currentLayer);
            world.FrameProfiler.Mark("doughform-regenmesh");
            world.BlockAccessor.MarkBlockDirty(__instance.Pos);
            world.BlockAccessor.MarkBlockEntityDirty(__instance.Pos);

            if (!HarmonyReflectionExtensions.CallMethod<bool>(__instance, "HasAnyVoxel"))
            {
                __instance.AvailableVoxels = 0;
                ___workItemStack = default!;
                world.BlockAccessor.SetBlock(0, __instance.Pos);
                return false;
            }
            __instance.CheckIfFinished(byPlayer, currentLayer);
            world.FrameProfiler.Mark("doughform-checkfinished");
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