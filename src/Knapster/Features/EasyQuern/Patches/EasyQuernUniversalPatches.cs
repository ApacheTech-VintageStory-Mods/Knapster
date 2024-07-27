using ApacheTech.VintageMods.Knapster.Features.EasyQuern.Systems;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Patches;

[HarmonySidedPatch(EnumAppSide.Universal)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyQuernUniversalPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityQuern), nameof(BlockEntityQuern.GrindSpeed), MethodType.Getter)]
    public static void Harmony_BlockEntityQuern_GrindSpeed_Getter_Postfix(Dictionary<string, long> ___playersGrinding, ref float __result)
    {
        __result *= GetSpeedMultiplier([.. ___playersGrinding.Keys]);
    }

    public static float GetSpeedMultiplier(List<string> players)
    {
        if (players.Count == 0 && !IncludeAutomated()) return 1f;
        return !EnabledForAll(players) ? 1f : SpeedMultiplier;
    }

    private static float SpeedMultiplier = ApiEx.OneOf(
        EasyQuernClient.Settings.SpeedMultiplier,
        EasyQuernServer.Settings.SpeedMultiplier);

    private static bool IncludeAutomated() => ApiEx.OneOf(
        EasyQuernClient.Settings.IncludeAutomated,
        EasyQuernServer.Settings.IncludeAutomated);

    private static bool EnabledForAll(IEnumerable<string> players) => !ApiEx.Return(
        () => EasyQuernClient.Settings.Enabled,
        () => EasyQuernServer.IsEnabledForAll(players));
}