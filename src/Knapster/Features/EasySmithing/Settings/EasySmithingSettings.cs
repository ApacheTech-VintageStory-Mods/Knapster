namespace Knapster.Features.EasySmithing.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasySmithingClientSettings))]
public class EasySmithingSettings : FeatureSettings<EasySmithingServerSettings>
{
    /// <summary>
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(2)]
    public int CostPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(3)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(4)]
    public bool InstantComplete { get; set; }
}