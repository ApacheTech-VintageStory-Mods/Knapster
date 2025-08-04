using Knapster.Features.EasySmithing.DataStructures;
using Knapster.Features.EasySmithing.Patches;

namespace XKnapster.Features.XSkillsCompatiblity;

[HarmonyUniversalPatch]
public static class XSkillsMetalworkingPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EasySmithingUniversalPatches), nameof(EasySmithingUniversalPatches.ProcessHit))]
    public static void Harmony_EasySmithingUniversalPatches_ProcessHit_Postfix(BlockEntityAnvil anvil, AnvilHitResult __result)
    {
        switch (__result.Action)
        {
            case AnvilHitAction.MetalMoved:
            case AnvilHitAction.SlagRemoved:
                var currentHitCount = anvil.GetHitCount();
                var newHitCount = currentHitCount + __result.Moves;
                anvil.SetHitCount(newHitCount);
                break;
            case AnvilHitAction.MetalSplit:
                var currentSplitCount = anvil.GetSplitCount();
                var newSplitCount = currentSplitCount + __result.Moves;
                anvil.SetSplitCount(newSplitCount);
                break;
            case AnvilHitAction.Nothing:
            case AnvilHitAction.ItemCompleted:
            default:
                // No special handling needed for these actions.
                break;
        }
    }
}