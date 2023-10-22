using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyHarvestingPacket
{
    /// <summary>
    ///     Determines whether the EasyPanning feature should be used.
    /// </summary>
    public required bool Enabled { get; init; }

    /// <summary>
    ///     Determines the speed multiplier, when using the EasyHarvesting feature.
    /// </summary>
    public required float SpeedMultiplier { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="EasyHarvestingPacket"/> class.
    /// </summary>
    public static EasyHarvestingPacket Create(bool enabled, float speedMultiplier)
    {
        return new EasyHarvestingPacket
        {
            Enabled = enabled,
            SpeedMultiplier = speedMultiplier
        };
    }
}