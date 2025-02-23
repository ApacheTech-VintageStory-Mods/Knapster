using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Settings;

[ProtoContract]
public class EasyDoughFormingClientSettings : EasyDoughFormingSettings, IEasyXClientSettings 
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}