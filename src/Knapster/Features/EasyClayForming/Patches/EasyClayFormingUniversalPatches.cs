using Knapster.Features.EasyClayForming.Extensions;
using Knapster.Features.EasyClayForming.Systems;

namespace Knapster.Features.EasyClayForming.Patches;

[HarmonyUniversalPatch]
public class EasyClayFormingUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockEntityClayForm), nameof(BlockEntityClayForm.OnUseOver), typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool))]
    public static bool UniversalPatch_BlockEntityClayForm_OnUseOver_Prefix(BlockEntityClayForm __instance,
        IPlayer byPlayer, bool mouseBreakMode, Vec3i voxelPos, BlockFacing facing, ref ItemStack ___workItemStack)
    {
        var voxelsPerClick = G.ApiEx.Return(
            _ => EasyClayFormingClient.Instance.Settings.VoxelsPerClick,
            _ => EasyClayFormingServer.Instance.Settings.VoxelsPerClick);

        var instantComplete = G.ApiEx.Return(
            _ => EasyClayFormingClient.Instance.Settings.InstantComplete,
            _ => EasyClayFormingServer.Instance.Settings.InstantComplete);

        var enabled = G.ApiEx.Return(
            _ => EasyClayFormingClient.Instance.Settings.Enabled,
            _ => EasyClayFormingServer.Instance.IsEnabledFor(byPlayer));

        try
        {
            var slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack is null || !__instance.CanWorkCurrent) return true;
            if (slot.Itemstack.Collectible is not ItemClay clay) return true;
            var blockSel = new BlockSelection { Position = __instance.Pos };
            var toolMode = clay.GetToolMode(slot, byPlayer, blockSel);

            if (!enabled)
            {
                if (toolMode > 3) clay.SetToolMode(slot, byPlayer, blockSel, 0);
                return true;
            }

            if (toolMode < 4) return true;
            if (mouseBreakMode) return false;

            if (__instance.Api.Side.IsClient())
            {
                __instance.CallMethod("SendUseOverPacket", byPlayer, voxelPos, facing, false);
            }

            clay.SetToolMode(slot, byPlayer, blockSel, 0);

            var currentLayer = __instance.CurrentLayer();
            if (instantComplete
                    ? __instance.CompleteInTurn(slot, byPlayer)
                    : __instance.AutoCompleteLayer(currentLayer, voxelsPerClick))
            {
                __instance.Api.World.PlaySoundAt(new AssetLocation("sounds/player/clayform.ogg"), byPlayer, byPlayer, true, 8f);
            }
            __instance.Api.World.FrameProfiler.Mark("clayform-modified");
            currentLayer = __instance.CurrentLayer();
            __instance.CallMethod("RegenMeshAndSelectionBoxes", currentLayer);
            __instance.Api.World.FrameProfiler.Mark("clayform-regenmesh");
            __instance.Api.World.BlockAccessor.MarkBlockDirty(__instance.Pos);
            __instance.Api.World.BlockAccessor.MarkBlockEntityDirty(__instance.Pos);

            if (!__instance.CallMethod<bool>("HasAnyVoxel"))
            {
                __instance.AvailableVoxels = 0;
                ___workItemStack = default!;
                __instance.Api.World.BlockAccessor.SetBlock(0, __instance.Pos);
                return false;
            }
            __instance.CheckIfFinished(byPlayer, currentLayer);
            __instance.Api.World.FrameProfiler.Mark("clayform-checkfinished");
            __instance.MarkDirty();

            if (slot.Itemstack is null) return false;
            clay.SetToolMode(slot, byPlayer, blockSel, 4);
            return false;
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
            return true;
        }
    }
}