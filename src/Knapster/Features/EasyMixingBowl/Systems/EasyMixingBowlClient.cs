using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Extensions;

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
        if (!api.ModLoader.IsModEnabled("aculinaryartillery"))
        {
            G.Logger.Warning("EasyMixingBowl is disabled because A Culinary Artillery mod is not loaded.");
            return false;
        }
        return base.ShouldLoad(api);
    }
}