namespace Knapster.Features.EasyDoughForming.Extensions;

/// <summary>
///     Provides extension methods for manipulating dough forming block entities and related voxel operations in Vintage Story mods.
/// </summary>
public static class BlockEntityDoughFormExtensions
{
    private static MethodInfo? _onAdd;

    /// <summary>
    ///     Adds a voxel to the dough form at the specified position and layer.
    /// </summary>
    /// <param name="block">The dough form block entity.</param>
    /// <param name="y">The Y layer.</param>
    /// <param name="pos">The voxel position.</param>
    /// <param name="radius">The radius for the operation.</param>
    public static void AddVoxel(BlockEntity block, int y, Vec3i pos, int radius)
    {
        _onAdd ??= AccessTools.Method(block.GetType(), "OnAdd", [typeof(int), typeof(Vec3i), typeof(int)]);
        _onAdd?.Invoke(block, [y, pos, radius]);
    }

    /// <summary>
    ///     Completes the dough form in turn, consuming dough from the provided item slot as needed.
    /// </summary>
    /// <param name="block">The dough form block entity.</param>
    /// <param name="itemSlot">The item slot containing dough.</param>
    /// <returns>True if the operation was successful; otherwise, false.</returns>
    public static bool CompleteInTurn(dynamic block, ItemSlot itemSlot)
    {
        for (var y = 0; y < 16; y++)
        {
            for (var x = 0; x < 16; x++)
            {
                for (var z = 0; z < 16; z++)
                {
                    if (block.SelectedRecipe is null) return false;
                    if (itemSlot.Empty) return false;

                    if (block.SelectedRecipe.Voxels[x, y, z])
                    {
                        if (block.Voxels[x, y, z]) continue;
                        AddVoxel(block, y, new Vec3i(x, y, z), 0);
                    }
                    else
                    {
                        if (!block.Voxels[x, y, z]) continue;
                        AccessToolsEx.CallMethod(block, "OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                    }

                    if (block.AvailableVoxels > 0) continue;
                    itemSlot.TakeOut(1);
                    block.AvailableVoxels += 25;
                }
            }
        }
        return true;
    }

    /// <summary>
    ///     Converts a 3D boolean voxel array to a 3D byte array.
    /// </summary>
    /// <param name="voxels">The 3D boolean voxel array.</param>
    /// <returns>A 3D byte array representing the voxels.</returns>
    public static byte[,,] ToBytes(this bool[,,] voxels)
    {
        var retVal = new byte[16, 16, 16];
        for (var y = 0; y < 16; y++)
        {
            for (var x = 0; x < 16; x++)
            {
                for (var z = 0; z < 16; z++)
                {
                    retVal[x, y, z] = (byte)(voxels[x, y, z] ? 1 : 0);
                }
            }
        }
        return retVal;
    }

    /// <summary>
    ///     Extracts a single layer from a 3D boolean voxel array.
    /// </summary>
    /// <param name="voxels">The 3D boolean voxel array.</param>
    /// <param name="y">The Y layer to extract.</param>
    /// <returns>A 2D boolean array representing the specified layer.</returns>
    public static bool[,] ForSingleLayer(this bool[,,] voxels, int y)
    {
        var retVal = new bool[16, 16];
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                retVal[x, z] = voxels[x, y, z];
            }
        }
        return retVal;
    }

    /// <summary>
    ///     Determines the current incomplete layer in the dough form.
    /// </summary>
    /// <param name="obj">The dough form block entity.</param>
    /// <param name="layerStart">The starting layer index.</param>
    /// <returns>The index of the first incomplete layer, or 16 if all layers are complete.</returns>
    public static int CurrentLayer(this object obj, int layerStart = 0)
    {
        dynamic block = obj;
        if (block.SelectedRecipe is null) return 0;
        for (var y = layerStart; y < 16; y++)
        {
            for (var x = 0; x < 16; x++)
            {
                for (var z = 0; z < 16; z++)
                {
                    if (block.Voxels[x, y, z] != block.SelectedRecipe.Voxels[x, y, z])
                    {
                        return y;
                    }
                }
            }
        }
        return 16;
    }

    /// <summary>
    ///     Automatically completes a specific layer of the dough form, up to a specified number of voxels.
    /// </summary>
    /// <param name="obj">The dough form block entity.</param>
    /// <param name="y">The Y layer to complete.</param>
    /// <param name="voxels">The maximum number of voxels to complete.</param>
    /// <returns>True if any voxels were changed; otherwise, false.</returns>
    public static bool AutoCompleteLayer(this object obj, int y, int voxels)
    {
        dynamic block = obj;
        if (y >= 16) return false;
        var result = false;
        var num = Math.Max(1, voxels);
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                var expected = block.SelectedRecipe?.Voxels[x, y, z] ?? false;
                var actual = block.Voxels[x, y, z];
                if (expected == actual) continue;
                result = true;

                if (expected)
                {
                    AddVoxel(block, y, new Vec3i(x, y, z), 0);
                }
                else
                {
                    AccessToolsEx.CallMethod(block, "OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                }

                if (--num == 0) return true;
            }
        }
        return result;
    }
}