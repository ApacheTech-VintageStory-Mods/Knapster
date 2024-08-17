namespace ApacheTech.VintageMods.Knapster.Features.EasyTilling.Settings;

public interface IEasyTillingSettings
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of Tilling with a hoe, when using the EasyTilling feature.
    /// </summary>
    float SpeedMultiplier { get; set; }
}