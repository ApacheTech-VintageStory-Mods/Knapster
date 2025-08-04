using Knapster.Features.EasyDoughForming.Settings;

namespace Knapster.Features.EasyDoughForming.Systems;

public sealed class EasyDoughFormingClient : EasyXClientSystemBase<EasyDoughFormingClient, EasyDoughFormingClientSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts");
}