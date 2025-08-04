namespace Knapster.Features.EasyTilling.Settings;

[ProtoContract]
public class EasyTillingClientSettings : EasyTillingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}