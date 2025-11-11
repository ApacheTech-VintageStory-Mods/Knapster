namespace Knapster.Features.EasySmithing.Systems;

public class EasySmithingClient : EasyXClientSystemBase<EasySmithingClient, EasySmithingClientSettings, EasySmithingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new SmithingGuiTab(Core, ServerSettings));
    }
}