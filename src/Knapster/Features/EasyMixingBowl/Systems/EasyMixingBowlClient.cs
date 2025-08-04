using Knapster.Features.EasyMixingBowl.Settings;

namespace Knapster.Features.EasyMixingBowl.Systems;

public sealed class EasyMixingBowlClient : EasyXClientSystemBase<EasyMixingBowlClient, EasyMixingBowlClientSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.IsModEnabled("aculinaryartillery");
}