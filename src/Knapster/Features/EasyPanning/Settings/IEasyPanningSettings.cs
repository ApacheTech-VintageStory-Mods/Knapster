namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings;

[UsedImplicitly(ImplicitUseTargetFlags.All)]
public interface IEasyPanningSettings
{
    /// <summary>
    ///     The time, in seconds, it takes to pan one layer of material.
    /// </summary>
    float SecondsPerLayer { get; set; }

    /// <summary>
    ///     The number of drops spawned per layer of material.
    /// </summary>
    int DropsPerLayer { get; set; }

    /// <summary>
    ///     The saturation lost when panning a single layer of material.
    /// </summary>
    float SaturationPerLayer { get; set; }
}