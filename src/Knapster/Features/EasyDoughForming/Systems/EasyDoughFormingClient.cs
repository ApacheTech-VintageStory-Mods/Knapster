namespace Knapster.Features.EasyDoughForming.Systems;

public sealed class EasyDoughFormingClient : EasyXClientSystemBase<EasyDoughFormingClient, EasyDoughFormingClientSettings, EasyDoughFormingServerSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
    {
        if (!api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts"))
        {
            G.Logger.Warning("EasyDoughForming is disabled because Art of Cooking and Core of Arts mods are not loaded.");
            return false;
        }
        return base.ShouldLoad(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new DoughFormingGuiTab(ServerSettings));
        base.StartClientSide(api);
    }
}