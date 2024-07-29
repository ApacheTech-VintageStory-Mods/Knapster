using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyQuernClientSettings : IEasyXClientSettings<IEasyQuernSettings>, IEasyQuernSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public float SpeedMultiplier { get; set; } = 1f;

    /// <inheritdoc />
    public bool IncludeAutomated { get; set; } = false;

    /// <inheritdoc />
    public bool StickyMouseButton { get; set; } = false;
}