namespace Knapster.Features.EasyKnapping.Systems;

public sealed class EasyKnappingClient : EasyXClientSystemBase<EasyKnappingClient, EasyKnappingClientSettings, EasyKnappingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new KnappingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}