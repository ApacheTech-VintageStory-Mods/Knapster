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
        __result *= SpeedMultiplier([.. ___playersGrinding.Keys]);
    }

    public static float SpeedMultiplier(List<string> players)
    {
        if (players.Count == 0 && !ApiEx.OneOf(
                EasyQuernClient.Settings.IncludeAutomated,
                EasyQuernServer.Settings.IncludeAutomated))
        {
            return 1f;
        }

        if (!ApiEx.Return(
                () => EasyQuernClient.Settings.Enabled,
                () => EasyQuernServer.IsEnabledForAll(players)))
        {
            return 1f;
        }

        return ApiEx.OneOf(
            EasyQuernClient.Settings.SpeedMultiplier,
            EasyQuernServer.Settings.SpeedMultiplier);
    }
}