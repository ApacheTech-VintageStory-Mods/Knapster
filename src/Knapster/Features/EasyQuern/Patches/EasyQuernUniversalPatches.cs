using ApacheTech.VintageMods.Knapster.Features.EasyQuern.Systems;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Patches;

[HarmonySidedPatch(EnumAppSide.Universal)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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

    private static bool StickyMouseButton => ApiEx.OneOf(
        EasyQuernClient.Settings.StickyMouseButton,
        EasyQuernServer.Settings.StickyMouseButton);

    private static float SpeedMultiplier => ApiEx.OneOf(
        EasyQuernClient.Settings.SpeedMultiplier,
        EasyQuernServer.Settings.SpeedMultiplier);

    private  static bool IncludeAutomated() => ApiEx.OneOf(
        EasyQuernClient.Settings.IncludeAutomated,
        EasyQuernServer.Settings.IncludeAutomated);

    private static bool EnabledForAll(IEnumerable<string> players) => !ApiEx.Return(
        () => EasyQuernClient.Settings.Enabled,
        () => EasyQuernServer.IsEnabledForAll(players));
}