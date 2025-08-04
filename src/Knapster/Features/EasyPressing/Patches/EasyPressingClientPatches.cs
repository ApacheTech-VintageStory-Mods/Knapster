using Knapster.Features.EasyPressing.Systems;

namespace Knapster.Features.EasyPressing.Patches;

[HarmonyClientSidePatch]
public sealed class EasyPressingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityFruitPress), nameof(BlockEntityFruitPress.CanUnscrew), MethodType.Getter)]
    public static void Harmony_Client_BlockEntityFruitPress_CanUnscrew_Getter_Postfix(ref bool __result)
    {
        __result = __result || EasyPressingClient.Instance.Settings.Enabled;
    }
}