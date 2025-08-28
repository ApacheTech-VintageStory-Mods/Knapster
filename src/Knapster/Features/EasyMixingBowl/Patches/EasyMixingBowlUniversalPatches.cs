namespace Knapster.Features.EasyMixingBowl.Patches;

[RequiresMod("aculinaryartillery")]
[HarmonyUniversalPatch]
public class EasyMixingBowlUniversalACulinaryArtilleryPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("ACulinaryArtillery.BlockEntityMixingBowl", "MixSpeed", MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersMixing, ref float __result)
    {
        __result *= G.CommandProcessor.Handle(new GetMixingBowlSpeedMultiplierCommand([.. ___playersMixing.Keys])).SpeedMultiplier;
    }
}