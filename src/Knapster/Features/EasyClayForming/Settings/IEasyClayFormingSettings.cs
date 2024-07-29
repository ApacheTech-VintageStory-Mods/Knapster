namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Settings;

[UsedImplicitly(ImplicitUseTargetFlags.All)]
public interface IEasyClayFormingSettings
{
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy ClayForming feature.
    /// </summary>
    int VoxelsPerClick { get; set; }

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    bool InstantComplete { get; set; }
}