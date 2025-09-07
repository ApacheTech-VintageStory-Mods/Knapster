using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Extensions;

namespace Knapster.Features.EasyPanning.Systems;

public sealed class EasyPanningClient : EasyXClientSystemBase<EasyPanningClient, EasyPanningClientSettings, EasyPanningServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new PanningGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}