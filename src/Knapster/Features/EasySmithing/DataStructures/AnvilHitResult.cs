namespace Knapster.Features.EasySmithing.DataStructures;

/// <summary>
///     Represents the result of an anvil hit action during smithing, including the action performed, voxel position, material, and number of moves.
/// </summary>
/// <param name="Player">The player who performed the anvil hit.</param>
/// <param name="Action">The action that occurred as a result of the anvil hit.</param>
/// <param name="VoxelPos">The position of the affected voxel, if applicable.</param>
/// <param name="Material">The material of the affected voxel, if applicable.</param>
/// <param name="Moves">The number of moves performed during the hit. Default is 1.</param>
public record AnvilHitResult(
    AnvilHitAction Action,
    IPlayer? Player = null,
    Vec3i? VoxelPos = null,
    EnumVoxelMaterial? Material = null,
    int Moves = 1
);