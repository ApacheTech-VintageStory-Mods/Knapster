namespace Knapster.Features.EasyHarvesting.Settings;

[ProtoContract]
public class EasyHarvestingClientSettings : EasyHarvestingSettings, IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}