using Knapster.Features.EasyMixingBowl.Systems;

namespace Knapster.Features.EasyMixingBowl.Patches;

[RequiresMod("aculinaryartillery")]
[HarmonyUniversalPatch]
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
        if (players.Count == 0 && !G.ApiEx.OneOf(
                EasyMixingBowlClient.Instance.Settings.IncludeAutomated,
                EasyMixingBowlServer.Instance.Settings.IncludeAutomated))
        {
            return 1f;
        }

        if (!G.ApiEx.Return(
                () => EasyMixingBowlClient.Instance.Settings.Enabled,
                () => EasyMixingBowlServer.Instance.IsEnabledForAll(players)))
        {
            return 1f;
        }

        return G.ApiEx.OneOf(
            EasyMixingBowlClient.Instance.Settings.SpeedMultiplier,
            EasyMixingBowlServer.Instance.Settings.SpeedMultiplier);
    }
}