namespace Knapster.Features.EasyPressing.Patches;

[HarmonyClientPatch]
public sealed class EasyPressingClientPatches
{
    [HarmonyPostfix]
    [HarmonyClientPatch(typeof(BlockEntityFruitPress), nameof(BlockEntityFruitPress.CanUnscrew), MethodType.Getter)]
    public static void Harmony_Client_BlockEntityFruitPress_CanUnscrew_Getter_Postfix(ref bool __result)
    {
        __result = __result || G.Services.GetRequiredService<EasyPressingClient>().Settings.Enabled;
    }
}