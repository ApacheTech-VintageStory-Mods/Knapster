using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyHarvestingClientSettings))]
public abstract class EasyHarvestingSettings : FeatureSettings
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyHarvesting feature.
    /// </summary>
    [ProtoMember(2)]
    public float SpeedMultiplier { get; set; } = 1f;
}