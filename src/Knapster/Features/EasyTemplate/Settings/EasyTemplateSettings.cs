using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyTemplate.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyTemplateClientSettings))]
public class EasyTemplateSettings : FeatureSettings<EasyTemplateServerSettings>
{
    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Template feature.
    /// </summary>
    [ProtoMember(2)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(3)]
    public bool InstantComplete { get; set; }
}