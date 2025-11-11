namespace Knapster.Features.EasyClayForming.Systems;

public sealed class EasyClayFormingClient : EasyXClientSystemBase<EasyClayFormingClient, EasyClayFormingClientSettings, EasyClayFormingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new ClayFormingGuiTab(Core, ServerSettings));
    }
}