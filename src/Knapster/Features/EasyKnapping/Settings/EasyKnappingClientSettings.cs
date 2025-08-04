namespace Knapster.Features.EasyKnapping.Settings;

[ProtoContract]
public class EasyKnappingClientSettings : EasyKnappingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}