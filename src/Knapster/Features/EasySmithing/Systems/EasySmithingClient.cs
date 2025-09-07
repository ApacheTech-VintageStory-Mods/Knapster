using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Extensions;

namespace Knapster.Features.EasySmithing.Systems;

public class EasySmithingClient : EasyXClientSystemBase<EasySmithingClient, EasySmithingClientSettings, EasySmithingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new SmithingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}