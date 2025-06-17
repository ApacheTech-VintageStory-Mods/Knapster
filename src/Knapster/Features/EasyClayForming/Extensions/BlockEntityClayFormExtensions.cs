namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Extensions;

/// <summary>
///     Provides extension methods for manipulating clay forming block entities and related voxel operations in Vintage Story mods.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public static class BlockEntityClayFormExtensions
{
    /// <summary>
    ///     Automatically completes the clay form using the selected recipe.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <returns>True if the operation was successful; otherwise, false.</returns>
    public static bool AutoComplete(this BlockEntityClayForm block)
    {
        if (block.SelectedRecipe is null) return false;
        block.Voxels = block.SelectedRecipe.Voxels;
        return true;
    }

    /// <summary>
    ///     Adds a voxel to the clay form at the specified position and layer.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <param name="y">The Y layer.</param>
    /// <param name="pos">The voxel position.</param>
    /// <param name="radius">The radius for the operation.</param>
    public static void AddVoxel(this BlockEntityClayForm block, int y, Vec3i pos, int radius)
    {
        var method = AccessTools.Method(typeof(BlockEntityClayForm), "OnAdd", [typeof(int), typeof(Vec3i), typeof(int)]);
        method?.Invoke(block, [y, pos, radius]);
    }

    /// <summary>
    ///     Completes the clay form in turn, consuming clay from the provided item slot as needed.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <param name="itemSlot">The item slot containing clay.</param>
    /// <returns>True if the operation was successful; otherwise, false.</returns>
    public static bool CompleteInTurn(this BlockEntityClayForm block, ItemSlot itemSlot)
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

    /// <summary>
    ///     Calculates the total clay cost required to complete the clay form.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <returns>The total clay cost, or -1 if no recipe is selected.</returns>
    public static int TotalClayCost(this BlockEntityClayForm block)
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

    /// <summary>
    ///     Automatically completes the knapping surface using the selected recipe for a single layer.
    /// </summary>
    /// <param name="block">The knapping surface block entity.</param>
    public static void AutoComplete(this BlockEntityKnappingSurface block)
    {
        block.Voxels = block.SelectedRecipe.Voxels.ForSingleLayer(0);
    }

    /// <summary>
    ///     Automatically completes the anvil using the selected recipe.
    /// </summary>
    /// <param name="block">The anvil block entity.</param>
    public static void AutoComplete(this BlockEntityAnvil block)
    {
        block.Voxels = block.SelectedRecipe.Voxels.ToBytes();
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
    ///     Determines the current incomplete layer in the clay form.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <param name="layerStart">The starting layer index.</param>
    /// <returns>The index of the first incomplete layer, or 16 if all layers are complete.</returns>
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

    /// <summary>
    ///     Automatically completes a specific layer of the clay form, up to a specified number of voxels.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
    /// <param name="y">The Y layer to complete.</param>
    /// <param name="voxels">The maximum number of voxels to complete.</param>
    /// <returns>True if any voxels were changed; otherwise, false.</returns>
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

    /// <summary>
    ///     Automatically completes the clay form by using selection boxes to determine voxel positions.
    /// </summary>
    /// <param name="block">The clay form block entity.</param>
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

    /// <summary>
    ///     Simulates a left mouse click on a specific selection box for the player.
    /// </summary>
    /// <param name="player">The player entity.</param>
    /// <param name="selectionBoxIndex">The index of the selection box.</param>
    public static void SimulateLeftClick(this IPlayer player, int selectionBoxIndex)
    {
        SimulateClick(player, selectionBoxIndex, false);
    }

    /// <summary>
    ///     Simulates a right mouse click on a specific selection box for the player.
    /// </summary>
    /// <param name="player">The player entity.</param>
    /// <param name="selectionBoxIndex">The index of the selection box.</param>
    public static void SimulateRightClick(this IPlayer player, int selectionBoxIndex)
    {
        SimulateClick(player, selectionBoxIndex, true);
    }

    /// <summary>
    ///     Simulates a mouse click (left or right) on a specific selection box for the player.
    /// </summary>
    /// <param name="player">The player entity.</param>
    /// <param name="selectionBoxIndex">The index of the selection box.</param>
    /// <param name="mouseBreakMode">True for right click (break), false for left click (interact).</param>
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
    /// <returns>The voxel position as a <see cref="Vec3i"/>.</returns>
    public static Vec3i GetVoxelPos(this Cuboidf cuboid)
    {
        return new Vec3i((int)(16f * cuboid.X1), (int)(16f * cuboid.Y1), (int)(16f * cuboid.Z1));
    }
}