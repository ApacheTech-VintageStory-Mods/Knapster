namespace Knapster.Features.EasyHarvesting.Systems;

public sealed class EasyHarvestingClient : EasyXClientSystemBase<EasyHarvestingClient, EasyHarvestingClientSettings, EasyHarvestingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new HarvestingGuiTab(Core, ServerSettings));
    }
}