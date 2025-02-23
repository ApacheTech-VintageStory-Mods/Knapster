using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Settings;
using Gantry.Core.Extensions.Api;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyDoughFormingClient : EasyXClientSystemBase<EasyDoughFormingClientSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts");
}