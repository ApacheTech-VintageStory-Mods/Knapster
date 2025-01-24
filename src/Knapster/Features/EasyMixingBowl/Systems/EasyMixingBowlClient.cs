using ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;
using Gantry.Services.EasyX.Abstractions;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyMixingBowlClient : EasyXClientSystemBase<EasyMixingBowlClientSettings>
{
    public override bool ShouldLoad(EnumAppSide forSide)
    {
        try
        {
            var shouldLoad = base.ShouldLoad(forSide) && UApi.ModLoader.IsModEnabled("aculinaryartillery");
            return shouldLoad;
        }
        catch
        {
            return false;
        }
    }
}