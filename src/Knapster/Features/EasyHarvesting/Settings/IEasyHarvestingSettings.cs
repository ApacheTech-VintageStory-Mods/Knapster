namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;

public interface IEasyHarvestingSettings
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyHarvesting feature.
    /// </summary>
    float SpeedMultiplier { get; set; }
}