using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyMixingBowlClientSettings))]
public abstract class EasyMixingBowlSettings : FeatureSettings
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