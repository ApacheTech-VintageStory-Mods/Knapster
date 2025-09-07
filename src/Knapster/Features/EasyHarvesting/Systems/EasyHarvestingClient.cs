using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Extensions;

namespace Knapster.Features.EasyHarvesting.Systems;

public sealed class EasyHarvestingClient : EasyXClientSystemBase<EasyHarvestingClient, EasyHarvestingClientSettings, EasyHarvestingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new HarvestingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}