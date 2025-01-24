using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyTilling.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyTillingClientSettings))]
public class EasyTillingSettings : FeatureSettings<EasyTillingServerSettings>
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of Tilling with a hoe, when using the EasyTilling feature.
    /// </summary>
    [ProtoMember(2)]
    public float SpeedMultiplier { get; set; } = 1f;
}