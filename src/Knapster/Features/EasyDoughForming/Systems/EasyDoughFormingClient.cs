namespace Knapster.Features.EasyDoughForming.Systems;

public sealed class EasyDoughFormingClient : EasyXClientSystemBase<EasyDoughFormingClient, EasyDoughFormingClientSettings, EasyDoughFormingServerSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
    {
        if (!api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts")) return false;
        return base.ShouldLoad(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new DoughFormingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}