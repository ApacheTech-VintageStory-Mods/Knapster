using Knapster.Features.EasyQuern.Systems;

namespace Knapster.Features.EasyQuern.Patches;

[HarmonyUniversalPatch]
public class EasyQuernUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockQuern), nameof(BlockQuern.OnBlockInteractCancel))]
    public static bool Harmony_BlockQuern_OnBlockInteractCancel_Prefix(EnumItemUseCancelReason cancelReason) 
        => !(StickyMouseButton && cancelReason == EnumItemUseCancelReason.ReleasedMouse);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityQuern), nameof(BlockEntityQuern.GrindSpeed), MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersGrinding, ref float __result) 
        => __result *= GetSpeedMultiplier([.. ___playersGrinding.Keys]);

    public static float GetSpeedMultiplier(List<string> players) 
        => players.Count == 0 && !IncludeAutomated() ? 1f 
        : !EnabledForAll(players) ? 1f 
        : SpeedMultiplier;

    private static bool StickyMouseButton => G.ApiEx.OneOf(
        EasyQuernClient.Instance.Settings.StickyMouseButton,
        EasyQuernServer.Instance.Settings.StickyMouseButton);

    private static float SpeedMultiplier => G.ApiEx.OneOf(
        EasyQuernClient.Instance.Settings.SpeedMultiplier,
        EasyQuernServer.Instance.Settings.SpeedMultiplier);

    private  static bool IncludeAutomated() => G.ApiEx.OneOf(
        EasyQuernClient.Instance.Settings.IncludeAutomated,
        EasyQuernServer.Instance.Settings.IncludeAutomated);

    private static bool EnabledForAll(IEnumerable<string> players)
    {
        var enabled = G.ApiEx.Return(
            () => EasyQuernClient.Instance.Settings.Enabled,
            () => EasyQuernServer.Instance.IsEnabledForAll(players));

        return enabled;
    }
}