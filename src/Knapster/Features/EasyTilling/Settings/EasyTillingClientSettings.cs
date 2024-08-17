using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyTilling.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyTillingClientSettings : IEasyXClientSettings<IEasyTillingSettings>, IEasyTillingSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public float SpeedMultiplier { get; set; } = 1f;
}