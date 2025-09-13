namespace Knapster.Features.EasyKnapping.Settings;

[ProtoContract]
public class EasyKnappingSettings : FeatureSettings<EasyKnappingServerSettings>
{
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Knapping feature.
    /// </summary>
    [ProtoMember(1)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(2)]
    public bool InstantComplete { get; set; }
}