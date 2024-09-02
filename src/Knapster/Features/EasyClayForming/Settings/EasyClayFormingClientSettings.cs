using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Settings;

[ProtoContract]
public class EasyClayFormingClientSettings : EasyClayFormingSettings, IEasyXClientSettings 
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}