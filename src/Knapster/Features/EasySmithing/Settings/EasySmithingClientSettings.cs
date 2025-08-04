namespace Knapster.Features.EasySmithing.Settings;

[ProtoContract]
public class EasySmithingClientSettings : EasySmithingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}