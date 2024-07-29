namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Settings;

[UsedImplicitly(ImplicitUseTargetFlags.All)]
public interface IEasySmithingSettings
{
    /// <summary>
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    int CostPerClick { get; set; }

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Smithing feature.
    /// </summary>
    int VoxelsPerClick { get; set; }

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Smithing feature.
    /// </summary>
    bool InstantComplete { get; set; }
}