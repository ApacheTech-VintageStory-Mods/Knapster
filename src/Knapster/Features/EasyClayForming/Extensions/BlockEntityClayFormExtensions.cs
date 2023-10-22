namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Extensions;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class BlockEntityClayFormExtensions
{
    public static bool AutoComplete(this BlockEntityClayForm block)
    {
        if (block.SelectedRecipe is null) return false;
        block.Voxels = block.SelectedRecipe.Voxels;
        block.AvailableVoxels = 0;
        return true;
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
        var retVal = new byte[16,16,16];
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
        var retVal = new bool[,] {};
        for (var x = 0; x < 16; x++)
        {
            for (var z = 0; z < 16; z++)
            {
                retVal[x, z] = voxels[x, y, z];
            }
        }
        return retVal;
    }

    public static int CurrentLayer(this BlockEntityClayForm block, int layerStart = 0)
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

    public static bool AutoCompleteLayer(this BlockEntityClayForm block, int y, int voxels)
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
                block.Voxels[x, y, z] = expected;
                block.AvailableVoxels--;
                if (--num == 0) return true;
            }
        }
        return result;
    }

    public static void AutoCompleteBySelectionBoxes(this BlockEntityClayForm block)
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