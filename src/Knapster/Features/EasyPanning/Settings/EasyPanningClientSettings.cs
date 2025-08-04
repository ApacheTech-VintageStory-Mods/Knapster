namespace Knapster.Features.EasyPanning.Settings; 

[ProtoContract]
public class EasyPanningClientSettings : EasyPanningSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}