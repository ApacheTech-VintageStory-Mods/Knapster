namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping.Settings;

[UsedImplicitly(ImplicitUseTargetFlags.All)]
public interface IEasyKnappingSettings
{   
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Knapping feature.
    /// </summary>
    int VoxelsPerClick { get; set; }

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    bool InstantComplete { get; set; }
}