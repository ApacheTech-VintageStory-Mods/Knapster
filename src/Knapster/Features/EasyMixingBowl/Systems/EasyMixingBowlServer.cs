namespace Knapster.Features.EasyMixingBowl.Systems;

public class EasyMixingBowlServer : EasyXServerSystemBase<EasyMixingBowlServer, EasyMixingBowlServerSettings, EasyMixingBowlClientSettings>
{
    protected override string SubCommandName => "MixingBowl";

    public override bool ShouldLoad(ICoreAPI api)
    {
        // https://mods.vintagestory.at/aculinaryartillery
        if (!api.ModLoader.IsModEnabled("aculinaryartillery")) return false;
        return base.ShouldLoad(api);
    }
}