using System.Reflection.Emit;

namespace Knapster.Features.EasyTilling.Patches;

[HarmonyUniversalPatch]
public sealed class EasyTillingUniversalPatches
{
    [HarmonyTranspiler]
    [HarmonyUniversalPatch(typeof(ItemHoe), nameof(ItemScythe.OnHeldInteractStep))]
    public static IEnumerable<CodeInstruction> UniversalPatch_ItemHoe_OnHeldInteractStep_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var result = new List<CodeInstruction>();
        var codeInstructions = instructions.ToArray();

        for (var i = 0; i < codeInstructions.Length -1; i++)
        {
            var current = codeInstructions[i];
            var next = codeInstructions[i+1];

            result.Add(current);

            if (!ShouldIntercept(current, next)) continue;

            result.Add(new CodeInstruction(OpCodes.Ldarg_3));
            result.Add(CodeInstruction.Call(typeof(EasyTillingUniversalPatches), nameof(SpeedMultiplier), [typeof(EntityAgent)]));
            result.Add(new CodeInstruction(OpCodes.Mul));
        }

        result.Add(codeInstructions.Last());
        return result;
    }

    private static bool ShouldIntercept(CodeInstruction current, CodeInstruction next)
    {
        if (current.Is(OpCodes.Ldc_R4, 1f) && next.opcode == OpCodes.Clt) return true;
        if (current.Is(OpCodes.Ldc_R4, 0.35f) && next.opcode == OpCodes.Ble_Un) return true;
        if (current.Is(OpCodes.Ldc_R4, 0.35f) && next.opcode == OpCodes.Ble_Un_S) return true;
        if (current.Is(OpCodes.Ldc_R4, 0.6f) && next.opcode == OpCodes.Ble_Un_S) return true;
        if (current.Is(OpCodes.Ldc_R4, 0.75f) && next.opcode == OpCodes.Blt_S) return true;
        if (current.Is(OpCodes.Ldc_R4, 0.87f) && next.opcode == OpCodes.Bge_Un) return true;
        return false;
    }

    private static float SpeedMultiplier(EntityAgent byEntity)
    {
        if (byEntity is not EntityPlayer playerEntity) return 3f;

        if (!G.ApiEx.Return(
                () => EasyTillingClient.Instance.Settings.Enabled,
                () => EasyTillingServer.Instance.IsEnabledFor(playerEntity.Player)))
        {
            return 3f;
        }

        return G.ApiEx.Return(
            () => EasyTillingClient.Instance.Settings.SpeedMultiplier,
            () => EasyTillingServer.Instance.Settings.SpeedMultiplier);
    }
}