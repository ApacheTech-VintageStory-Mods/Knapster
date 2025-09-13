namespace Knapster.Features.EasyTilling.Systems;

public sealed class EasyTillingClient : EasyXClientSystemBase<EasyTillingClient, EasyTillingClientSettings, EasyTillingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new TillingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}