namespace Knapster.Features.EasyDoughForming.Systems;

public sealed class EasyDoughFormingClient : EasyXClientSystemBase<EasyDoughFormingClient, EasyDoughFormingClientSettings, EasyDoughFormingServerSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
    {
        // https://mods.vintagestory.at/coreofarts
        // https://mods.vintagestory.at/artofcooking
        if (!api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts")) return false;
        return base.ShouldLoad(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api.AddSettingsTab(() => new DoughFormingGuiTab(Core, ServerSettings));
    }
}