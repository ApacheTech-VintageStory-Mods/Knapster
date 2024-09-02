using ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;
using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings; 

[ProtoContract]
public class EasyPanningClientSettings : EasyPanningSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}