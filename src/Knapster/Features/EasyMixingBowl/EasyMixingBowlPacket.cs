using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyMixingBowlPacket
{
    /// <summary>
    ///     Determines whether the EasyMixingBowl Feature should be used.
    /// </summary>
    public required bool Enabled { get; init; }

    /// <summary>
    ///     Determines the multiplier to apply to the time it takes to craft something in a mixing bowl.
    /// </summary>
    public required float SpeedMultiplier { get; init; }

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated mixing bowls.
    /// </summary>
    public bool IncludeAutomated { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="EasyMixingBowlPacket"/> class.
    /// </summary>
    public static EasyMixingBowlPacket Create(bool enabled, float speedMultiplier, bool includeAutomated) => new()
    {
        Enabled = enabled,
        SpeedMultiplier = speedMultiplier,
        IncludeAutomated = includeAutomated
    };
}