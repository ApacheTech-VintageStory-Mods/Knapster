using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyKnappingClientSettings : IEasyXClientSettings<IEasyKnappingSettings>, IEasyKnappingSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public int VoxelsPerClick { get; set; } = 1;

    /// <inheritdoc />
    public bool InstantComplete { get; set; } = false;
}