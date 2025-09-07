using Knapster.Features.ModMenu.Extensions;

namespace Knapster.Features.EasyClayForming.Systems;

public sealed class EasyClayFormingClient : EasyXClientSystemBase<EasyClayFormingClient, EasyClayFormingClientSettings, EasyClayFormingServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new ClayFormingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}