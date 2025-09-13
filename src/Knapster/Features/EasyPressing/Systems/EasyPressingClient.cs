namespace Knapster.Features.EasyPressing.Systems;

public sealed class EasyPressingClient : EasyXClientSystemBase<EasyPressingClient, EasyPressingClientSettings, EasyPressingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new PressingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}