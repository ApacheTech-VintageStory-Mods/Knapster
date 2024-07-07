// ReSharper disable InconsistentNaming

using ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Systems;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Patches;

[RequiresMod("aculinaryartillery")]
[HarmonySidedPatch(EnumAppSide.Universal)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyMixingBowlUniversalACulinaryArtilleryPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("ACulinaryArtillery.BlockEntityMixingBowl", "MixSpeed", MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersMixing, ref float __result)
    {
        __result *= SpeedMultiplier([.. ___playersMixing.Keys]);
    }

    public static float SpeedMultiplier(List<string> players)
    {
        if (players.Count == 0 && !ApiEx.OneOf(
                EasyMixingBowlClient.Settings.IncludeAutomated,
                EasyMixingBowlServer.Settings.IncludeAutomated))
        {
            return 1f;
        }

        if (!ApiEx.Return(
                () => EasyMixingBowlClient.Settings.Enabled,
                () => EasyMixingBowlServer.IsEnabledForAll(players)))
        {
            return 1f;
        }

        return ApiEx.OneOf(
            EasyMixingBowlClient.Settings.SpeedMultiplier,
            EasyMixingBowlServer.Settings.SpeedMultiplier);
    }
}