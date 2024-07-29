using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyPanningClientSettings : IEasyXClientSettings<IEasyPanningSettings>, IEasyPanningSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public float SecondsPerLayer { get; set; } = 4f;

    /// <inheritdoc />
    public int DropsPerLayer { get; set; } = 1;

    /// <inheritdoc />
    public float SaturationPerLayer { get; set; } = 3f;
}