namespace Knapster.Features.EasyMixingBowl.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyMixingBowlClientSettings))]
public class EasyMixingBowlSettings : FeatureSettings<EasyMixingBowlServerSettings>
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyMixingBowl feature.
    /// </summary>
    [ProtoMember(2)]
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated mixing bowls.
    /// </summary>
    [ProtoMember(3)]
    public bool IncludeAutomated { get; set; }
}