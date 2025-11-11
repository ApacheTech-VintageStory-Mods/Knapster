namespace Knapster.Features.EasyMixingBowl.Patches;

[HarmonyUniversalPatch]
[RequiresMod("aculinaryartillery")]
public class EasyMixingBowlUniversalACulinaryArtilleryPatches
{
    [HarmonyPrefix]
    [RequiresMod("aculinaryartillery")]
    [HarmonyUniversalPatch("ACulinaryArtillery.BlockMixingBowl", "OnBlockInteractCancel")]
    public static bool Harmony_BlockMixingBowl_OnBlockInteractCancel_Prefix(EnumItemUseCancelReason cancelReason)
        => G.CommandProcessor.Handle(new StickyMixingBowlCommand(cancelReason)).Success;

    [HarmonyPostfix]
    [RequiresMod("aculinaryartillery")]
    [HarmonyUniversalPatch("ACulinaryArtillery.BlockEntityMixingBowl", "MixSpeed", MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersMixing, ref float __result)
    {
        __result *= G.CommandProcessor.Handle(new GetMixingBowlSpeedMultiplierCommand([.. ___playersMixing.Keys])).SpeedMultiplier;
    }
}