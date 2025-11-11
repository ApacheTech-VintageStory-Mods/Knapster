namespace Knapster.Features.EasyGrinding.Systems;

public class EasyGrindingClient : EasyXClientSystemBase<EasyGrindingClient, EasyGrindingClientSettings, EasyGrindingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new GrindingGuiTab(Core, ServerSettings));
    }
}