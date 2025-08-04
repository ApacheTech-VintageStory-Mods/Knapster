using Knapster.Features.EasyKnapping.Systems;

namespace Knapster.Features.EasyKnapping.Patches;

[HarmonyServerSidePatch]
public sealed class EasyKnappingServerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockEntityKnappingSurface), "OnUseOver", typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool))]
    public static bool ServerPatch_BlockEntityKnappingSurface_OnUseOver_Prefix(
        BlockEntityKnappingSurface __instance, IPlayer byPlayer)
    {
        try
        {
            if (!EasyKnappingServer.Instance.IsEnabledFor(byPlayer)) return true;
            if (byPlayer.Entity.Controls.CtrlKey) return true;
            if (__instance?.SelectedRecipe?.Voxels is null) return true;

            if (EasyKnappingServer.Instance.Settings.InstantComplete)
            {
                AutoComplete(__instance);
            }

            return true;
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
            return true;
        }
    }

    private static void AutoComplete(BlockEntityKnappingSurface blockEntity)
    {
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                blockEntity.Voxels[x, z] = blockEntity.SelectedRecipe.Voxels[x, 0, z];
            }
        }
    }
}