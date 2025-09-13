namespace Knapster.Features.EasyMixingBowl.Settings;

[ProtoContract]
public class EasyMixingBowlSettings : FeatureSettings<EasyMixingBowlServerSettings>
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of using the mixing bowl.
    /// </summary>
    [ProtoMember(1)]
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated mixing bowls.
    /// </summary>
    [ProtoMember(2)]
    public bool IncludeAutomated { get; set; }

    /// <summary>
    ///     When active, the user will not need to keep the right-mouse button held down to keep the mixing bowl active.
    /// </summary>
    [ProtoMember(3)]
    public bool StickyMouseButton { get; set; }
}