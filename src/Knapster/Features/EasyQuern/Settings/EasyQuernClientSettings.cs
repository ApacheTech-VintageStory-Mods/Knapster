using ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;
using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;

[ProtoContract]
public class EasyQuernClientSettings : EasyQuernSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}