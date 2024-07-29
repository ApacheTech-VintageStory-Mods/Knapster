namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;

[UsedImplicitly(ImplicitUseTargetFlags.All)]
public interface IEasyQuernSettings
{
    /// <summary>
    ///     Determines the multiplier to apply to the time it takes to craft something in a quern.
    /// </summary>
    float SpeedMultiplier { get; set; }

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    bool IncludeAutomated { get; set; }

    /// <summary>
    ///     When active, the user will not need to keep the right-mouse button held down to keep the quern active.
    /// </summary>
    bool StickyMouseButton { get; set; }
}