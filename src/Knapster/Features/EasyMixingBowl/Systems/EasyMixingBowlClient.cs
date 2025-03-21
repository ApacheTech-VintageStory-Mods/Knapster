using ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;
using Gantry.Services.EasyX.Abstractions;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyMixingBowlClient : EasyXClientSystemBase<EasyMixingBowlClientSettings>
{
    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.IsModEnabled("aculinaryartillery");
}