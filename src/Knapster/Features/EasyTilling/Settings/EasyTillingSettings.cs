namespace Knapster.Features.EasyTilling.Settings;

[ProtoContract]
public class EasyTillingSettings : FeatureSettings<EasyTillingServerSettings>
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of Tilling with a hoe, when using the EasyTilling feature.
    /// </summary>
    [ProtoMember(1)]
    public float SpeedMultiplier { get; set; } = 1f;
}