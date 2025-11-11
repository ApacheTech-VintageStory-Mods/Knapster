using System.Reflection.Emit;

namespace Knapster.Features.EasyHarvesting.Patches;

[HarmonyUniversalPatch]
public sealed class EasyHarvestingUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyUniversalPatch(typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStop))]
    public static void UniversalPatch_ItemScythe_OnHeldAttackStop_Prefix(ref float secondsPassed, EntityAgent byEntity)
    {
        secondsPassed /= SpeedMultiplier(byEntity);
    }

    [HarmonyTranspiler]
    [HarmonyUniversalPatch(typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStep))]
    public static IEnumerable<CodeInstruction> UniversalPatch_ItemScythe_OnHeldAttackStep_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var result = new List<CodeInstruction>();
        var codeInstructions = instructions.ToArray();

        for (var i = 0; i < codeInstructions.Length - 1; i++)
        {
            var current = codeInstructions[i];
            var next = codeInstructions[i + 1];

            result.Add(current);

            if (!(current.Is(OpCodes.Ldc_R4, 2f) && next.opcode == OpCodes.Clt)) continue;
            result.Add(new CodeInstruction(OpCodes.Ldarg_3));
            result.Add(CodeInstruction.Call(typeof(EasyHarvestingUniversalPatches), nameof(SpeedMultiplier), [typeof(EntityAgent)]));
            result.Add(new CodeInstruction(OpCodes.Mul));
        }

        result.Add(codeInstructions.Last());
        return result;
    }

    private static float SpeedMultiplier(EntityAgent byEntity)
        => G.CommandProcessor.Handle(new GetHarvestingSpeedMultiplierCommand(byEntity)).SpeedMultiplier;
}