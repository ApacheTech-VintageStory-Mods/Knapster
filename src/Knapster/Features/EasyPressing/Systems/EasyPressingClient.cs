namespace Knapster.Features.EasyPressing.Systems;

public sealed class EasyPressingClient : EasyXClientSystemBase<EasyPressingClient, EasyPressingClientSettings, EasyPressingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new PressingGuiTab(Core, ServerSettings));
    }
}