namespace Knapster.Features.EasyPanning.Systems;

public sealed class EasyPanningClient : EasyXClientSystemBase<EasyPanningClient, EasyPanningClientSettings, EasyPanningServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new PanningGuiTab(Core, ServerSettings));
    }
}