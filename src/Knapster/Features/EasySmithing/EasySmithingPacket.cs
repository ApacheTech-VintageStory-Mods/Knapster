using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasySmithingPacket
{
    /// <summary>
    ///     Determines whether the EasyClayForming Feature should be used.
    /// </summary>
    public required bool Enabled { get; init; }

    /// <summary>
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    public required int CostPerClick { get; init; }

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the EasyKnapping feature.
    /// </summary>
    public required int VoxelsPerClick { get; init; }

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Clay Forming feature.
    /// </summary>
    public bool InstantComplete { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="EasySmithingPacket"/> class.
    /// </summary>
    public static EasySmithingPacket Create(bool enabled, int voxelsPerClick, int costPerClick, bool instantComplete)
    {
        return new EasySmithingPacket
        {
            Enabled = enabled,
            CostPerClick = costPerClick,
            VoxelsPerClick = voxelsPerClick,
            InstantComplete = instantComplete
        };
    }
}