using ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;
using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Settings;

[ProtoContract]
public class EasySmithingClientSettings : EasySmithingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}