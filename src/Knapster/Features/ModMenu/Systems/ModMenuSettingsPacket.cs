namespace Knapster.Features.ModMenu.Systems;

[ProtoContract]
public sealed class ModMenuSettingsPacket
{
    [ProtoMember(1)]
    public ModFileScope Scope { get; set; }
}