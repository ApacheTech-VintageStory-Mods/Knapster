using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyTemplate.Settings;

[ProtoContract]
public class EasyTemplateClientSettings : EasyTemplateSettings, IEasyXClientSettings 
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}