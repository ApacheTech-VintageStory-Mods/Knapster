namespace Knapster.Features.EasyHarvesting.Systems;

public sealed class EasyHarvestingServer : EasyXServerSystemBase<EasyHarvestingServer, EasyHarvestingServerSettings, EasyHarvestingClientSettings>
{
    protected override string SubCommandName => "Harvesting";
}