using ApacheTech.VintageMods.Knapster.Features.EasyKnapping.Settings;
using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;

[ProtoContract]
public class EasyMixingBowlClientSettings : EasyMixingBowlSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}