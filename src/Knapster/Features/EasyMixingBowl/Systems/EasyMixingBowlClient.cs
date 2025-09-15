namespace Knapster.Features.EasyMixingBowl.Systems;

public sealed class EasyMixingBowlClient : EasyXClientSystemBase<EasyMixingBowlClient, EasyMixingBowlClientSettings, EasyMixingBowlServerSettings>
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        api.AddSettingsTab(() => new MixingBowlGuiTab(ServerSettings));
        base.StartClientSide(api);
    }

    public override bool ShouldLoad(ICoreAPI api)
    {
        if (!api.ModLoader.IsModEnabled("aculinaryartillery")) return false;
        return base.ShouldLoad(api);
    }
}