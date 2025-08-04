namespace Knapster.Features.EasyPressing.Settings;

[ProtoContract]
public class EasyPressingClientSettings : EasyPressingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}