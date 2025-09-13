namespace Knapster.Features.EasyClayForming.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyClayFormingClientSettings))]
[ProtoInclude(101, typeof(EasyClayFormingServerSettings))]
public class EasyClayFormingSettings : FeatureSettings<EasyClayFormingServerSettings>
{
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy ClayForming feature.
    /// </summary>
    [ProtoMember(1)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(2)]
    public bool InstantComplete { get; set; }
}