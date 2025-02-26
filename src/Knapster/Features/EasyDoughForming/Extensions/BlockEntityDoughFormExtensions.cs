using System.Reflection;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Extensions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class BlockEntityDoughFormExtensions
{
    private static MethodInfo _onAdd;

    public static void AddVoxel(BlockEntity block, int y, Vec3i pos, int radius)
    {
        _onAdd ??= AccessTools.Method(block.GetType(), "OnAdd", [typeof(int), typeof(Vec3i), typeof(int)]);
        _onAdd?.Invoke(block, [y, pos, radius]);
    }

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
                        HarmonyReflectionExtensions.CallMethod(block, "OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                    }

                    if (block.AvailableVoxels > 0) continue;
                    itemSlot.TakeOut(1);
                    block.AvailableVoxels += 25;
                }
            }
        }
        return true;
    }

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

    public static bool[,] ForSingleLayer(this bool[,,] voxels, int y)
    {
        var retVal = new bool[,] { };
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                retVal[x, z] = voxels[x, y, z];
            }
        }
        return retVal;
    }

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
                    HarmonyReflectionExtensions.CallMethod(block, "OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                }

                if (--num == 0) return true;
            }
        }
        return result;
    }
}