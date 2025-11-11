namespace Knapster.Features.EasyKnapping.Systems;

public sealed class EasyKnappingClient : EasyXClientSystemBase<EasyKnappingClient, EasyKnappingClientSettings, EasyKnappingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new KnappingGuiTab(Core, ServerSettings));
    }
}