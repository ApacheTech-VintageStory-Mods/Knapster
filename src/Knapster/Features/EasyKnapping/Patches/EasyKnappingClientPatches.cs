using Knapster.Features.EasyKnapping.Systems;

namespace Knapster.Features.EasyKnapping.Patches;

[HarmonyClientSidePatch]
public sealed class EasyKnappingClientPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockEntityKnappingSurface), "OnUseOver", typeof(IPlayer), typeof(int), typeof(BlockFacing), typeof(bool))]
    public static bool ClientPatch_BlockEntityKnappingSurface_OnUseOver_Prefix(
        BlockEntityKnappingSurface __instance, IPlayer byPlayer, BlockFacing facing, bool mouseMode)
    {
        try
        {
            if (!EasyKnappingClient.Instance.Settings.Enabled) return true;
            if (byPlayer.Entity.Controls.CtrlKey) return true;
            if (__instance?.SelectedRecipe?.Voxels is null) return true;

            for (var i = 0; i < EasyKnappingClient.Instance.Settings.VoxelsPerClick; i++)
            {
                if (!__instance.CallMethod<bool>("HasAnyVoxel")) return true;
                var voxelPos = FindNextVoxelToRemove(__instance);

                var method = AccessTools.Method(typeof(BlockEntityKnappingSurface), "OnUseOver",
                    [typeof(IPlayer), typeof(Vec3i), typeof(BlockFacing), typeof(bool)]);

                method.Invoke(__instance, [byPlayer, voxelPos, facing, mouseMode]);

            }
            return false;
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
            return true;
        }
    }

    private static Vec3i FindNextVoxelToRemove(BlockEntityKnappingSurface blockEntity)
    {
        if (blockEntity?.SelectedRecipe?.Voxels is null) return Vec3i.Zero;
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                if (!IsOutlineVoxel(blockEntity, x, z)) continue;
                if (VoxelNeedsRemoving(blockEntity, x, z))
                {
                    return new Vec3i(x, 0, z);
                }
            }
        }
        return Vec3i.Zero;
    }

    private static bool IsOutlineVoxel(BlockEntityKnappingSurface blockEntity, int x, int z)
    {
        if (blockEntity?.SelectedRecipe?.Voxels is null) return false;
        try
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    var xi = x + i;
                    if (xi is < 0 or >= 16) continue;

                    var zj = z + j;
                    if (zj is < 0 or >= 16) continue;

                    if (blockEntity.SelectedRecipe.Voxels[xi, 0, zj]) return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private static bool VoxelNeedsRemoving(BlockEntityKnappingSurface blockEntity, int x, int z)
    {
        return blockEntity.Voxels[x, z] != blockEntity.SelectedRecipe.Voxels[x, 0, z];
    }
}