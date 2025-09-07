using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Extensions;

namespace Knapster.Features.EasyGrinding.Systems;

public class EasyGrindingClient : EasyXClientSystemBase<EasyGrindingClient, EasyGrindingClientSettings, EasyGrindingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new GrindingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}