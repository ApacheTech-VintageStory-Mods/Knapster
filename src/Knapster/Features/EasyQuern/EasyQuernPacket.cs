using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyQuernPacket
{
    /// <summary>
    ///     Determines whether the EasyQuern Feature should be used.
    /// </summary>
    public required bool Enabled { get; init; }

    /// <summary>
    ///     Determines the multiplier to apply to the time it takes to craft something in a quern.
    /// </summary>
    public required float SpeedMultiplier { get; init; }

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    public bool IncludeAutomated { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="EasyQuernPacket"/> class.
    /// </summary>
    public static EasyQuernPacket Create(bool enabled, float speedMultiplier, bool includeAutomated) => new()
    {
        Enabled = enabled,
        SpeedMultiplier = speedMultiplier,
        IncludeAutomated = includeAutomated
    };
}