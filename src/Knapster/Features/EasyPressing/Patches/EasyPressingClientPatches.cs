using ApacheTech.VintageMods.Knapster.Features.EasyPressing.Systems;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Patches;

[HarmonyClientSidePatch]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyPressingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityFruitPress), nameof(BlockEntityFruitPress.CanUnscrew), MethodType.Getter)]
    public static void Harmony_Client_BlockEntityFruitPress_CanUnscrew_Getter_Postfix(ref bool __result)
    {
        __result = __result || EasyPressingClient.Settings.Enabled;
    }
}