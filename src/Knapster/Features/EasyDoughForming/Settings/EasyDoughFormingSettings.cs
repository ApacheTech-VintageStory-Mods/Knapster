using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyDoughFormingClientSettings))]
public class EasyDoughFormingSettings : FeatureSettings<EasyDoughFormingServerSettings>
{
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy DoughForming feature.
    /// </summary>
    [ProtoMember(2)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(3)]
    public bool InstantComplete { get; set; }
}