using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyPanningClientSettings))]
public abstract class EasyPanningSettings : FeatureSettings
{
    /// <summary>
    ///     The time, in seconds, it takes to pan one layer of material.
    /// </summary>
    [ProtoMember(2)]
    public float SecondsPerLayer { get; set; } = 4f;

    /// <summary>
    ///     The number of drops spawned per layer of material.
    /// </summary>
    [ProtoMember(3)]
    public int DropsPerLayer { get; set; } = 1;

    /// <summary>
    ///     The saturation lost when panning a single layer of material.
    /// </summary>
    [ProtoMember(4)]
    public float SaturationPerLayer { get; set; } = 3f;
}