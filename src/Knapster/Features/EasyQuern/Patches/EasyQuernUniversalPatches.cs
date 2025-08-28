namespace Knapster.Features.EasyQuern.Patches;

[HarmonyUniversalPatch]
public class EasyQuernUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockQuern), nameof(BlockQuern.OnBlockInteractCancel))]
    public static bool Harmony_BlockQuern_OnBlockInteractCancel_Prefix(EnumItemUseCancelReason cancelReason)
        => G.CommandProcessor.Handle(new StickyQuernCommand(cancelReason)).Success;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityQuern), nameof(BlockEntityQuern.GrindSpeed), MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersGrinding, ref float __result)
        => __result *= G.CommandProcessor.Handle(new GetQuernSpeedMultiplierCommand([.. ___playersGrinding.Keys])).SpeedMultiplier;
}