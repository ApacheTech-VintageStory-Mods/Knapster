using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyClayFormingClientSettings : IEasyXClientSettings<IEasyClayFormingSettings>, IEasyClayFormingSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public int VoxelsPerClick { get; set; } = 1;

    /// <inheritdoc />
    public bool InstantComplete { get; set; } = false;
}