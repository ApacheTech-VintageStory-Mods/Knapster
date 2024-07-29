namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;

public interface IEasyMixingBowlSettings
{
    /// <summary>
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyMixingBowl feature.
    /// </summary>
    float SpeedMultiplier { get; set; }

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated mixing bowls.
    /// </summary>
    bool IncludeAutomated { get; set; }
}