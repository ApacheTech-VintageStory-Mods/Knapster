using ArtOfCooking.BlockEntities;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Extensions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class BlockEntityDoughFormExtensions
{
    public static bool AutoComplete(this BlockEntityDoughForm block)
    {
        if (block.SelectedRecipe is null) return false;
        block.Voxels = block.SelectedRecipe.Voxels;
        return true;
    }

    public static void AddVoxel(this BlockEntityDoughForm block, int y, Vec3i pos, int radius)
    {
        var method = AccessTools.Method(typeof(BlockEntityDoughForm), "OnAdd", [typeof(int), typeof(Vec3i), typeof(int)]);
        method?.Invoke(block, [y, pos, radius]);
    }

    public static bool CompleteInTurn(this BlockEntityDoughForm block, ItemSlot itemSlot)
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
                        block.AddVoxel(y, new Vec3i(x, y, z), 0);
                    }
                    else
                    {
                        if (!block.Voxels[x, y, z]) continue;
                        block.CallMethod("OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                    }

                    if (block.AvailableVoxels > 0) continue;
                    itemSlot.TakeOut(1);
                    block.AvailableVoxels += 25;
                }
            }
        }
        return true;
    }

    public static int TotalDoughCost(this BlockEntityDoughForm block)
    {
        if (block.SelectedRecipe is null) return -1;

        var voxelsThatNeedFilling = 0;
        var voxelsThatNeedRemoving = 0;

        for (var y = 0; y < 16; y++)
        {
            for (var x = 0; x < 16; x++)
            {
                for (var z = 0; z < 16; z++)
                {
                    if (block.SelectedRecipe.Voxels[x, y, z])
                    {
                        if (!block.Voxels[x, y, z]) voxelsThatNeedFilling++;
                    }
                    else
                    {
                        if (block.Voxels[x, y, z]) voxelsThatNeedRemoving++;
                    }
                }
            }
        }

        return (voxelsThatNeedFilling - voxelsThatNeedRemoving) / 25;
    }

    public static void AutoComplete(this BlockEntityKnappingSurface block)
    {
        block.Voxels = block.SelectedRecipe.Voxels.ForSingleLayer(0);
    }

    public static void AutoComplete(this BlockEntityAnvil block)
    {
        block.Voxels = block.SelectedRecipe.Voxels.ToBytes();
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

    public static int CurrentLayer(this BlockEntityDoughForm block, int layerStart = 0)
    {
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

    public static bool AutoCompleteLayer(this BlockEntityDoughForm block, int y, int voxels)
    {
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
                    block.AddVoxel(y, new Vec3i(x, y, z), 0);
                }
                else
                {
                    block.CallMethod("OnRemove", y, new Vec3i(x, y, z), BlockFacing.DOWN, 0);
                }

                if (--num == 0) return true;
            }
        }
        return result;
    }

    public static void AutoCompleteBySelectionBoxes(this BlockEntityDoughForm block)
    {
        var selectionBoxes = block.GetField<Cuboidf[]>("selectionBoxes");

        for (var i = 0; i < block.SelectedRecipe.QuantityLayers - 1; i++)
        {
            foreach (var selectionBox in selectionBoxes)
            {
                var voxelPos = selectionBox.GetVoxelPos();
                var x = voxelPos.X;
                var y = voxelPos.Y;
                var z = voxelPos.Z;

                var expected = block.SelectedRecipe.Voxels[x, y, z];
                var actual = block.Voxels[x, y, z];
                if (expected == actual) continue;
                block.Voxels[x, y, z] = expected;
            }
        }
    }


    public static void SimulateLeftClick(this IPlayer player, int selectionBoxIndex)
    {
        SimulateClick(player, selectionBoxIndex, false);
    }


    public static void SimulateRightClick(this IPlayer player, int selectionBoxIndex)
    {
        SimulateClick(player, selectionBoxIndex, true);
    }


    public static void SimulateClick(this IPlayer player, int selectionBoxIndex, bool mouseBreakMode)
    {
        var blockSel = player.CurrentBlockSelection;
        if (blockSel is null) return;
        blockSel.SelectionBoxIndex = selectionBoxIndex;
        var slot = player.InventoryManager.ActiveHotbarSlot;
        var handling = EnumHandHandling.NotHandled;

        if (mouseBreakMode)
        {
            slot.Itemstack?.Item?.OnHeldAttackStart(slot, player.Entity, blockSel, player.CurrentEntitySelection, ref handling);
            return;
        }
        slot.Itemstack?.Item?.OnHeldInteractStart(slot, player.Entity, blockSel, player.CurrentEntitySelection, true, ref handling);
    }

    /// <summary>
    ///     Converts a specific cuboid to a specific voxel position, within a block.
    /// </summary>
    /// <param name="cuboid">The cuboid to convert.</param>
    public static Vec3i GetVoxelPos(this Cuboidf cuboid)
    {
        return new Vec3i((int)(16f * cuboid.X1), (int)(16f * cuboid.Y1), (int)(16f * cuboid.Z1));
    }
}