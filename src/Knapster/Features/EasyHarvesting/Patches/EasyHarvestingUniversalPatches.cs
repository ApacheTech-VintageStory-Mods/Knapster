using ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Systems;
using System.Reflection.Emit;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Patches;

[HarmonySidedPatch(EnumAppSide.Universal)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyHarvestingUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStop))]
    public static void UniversalPatch_ItemScythe_OnHeldAttackStop_Prefix(ref float secondsPassed, EntityAgent byEntity)
    {
        secondsPassed /= SpeedMultiplier(byEntity);
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ItemScythe), nameof(ItemScythe.OnHeldAttackStep))]
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
            result.Add(CodeInstruction.Call(typeof(EasyHarvestingUniversalPatches), nameof(SpeedMultiplier), new[] { typeof(EntityAgent) }));
            result.Add(new CodeInstruction(OpCodes.Mul));
        }

        result.Add(codeInstructions.Last());
        return result;
    }

    private static float SpeedMultiplier(EntityAgent byEntity)
    {
        if (byEntity is not EntityPlayer playerEntity) return 1f;

        if (!ApiEx.Return(
                () => EasyHarvestingClient.Settings.Enabled,
                () => EasyHarvestingServer.IsEnabledFor(playerEntity.Player)))
        {
            return 1f;
        }

        return ApiEx.OneOf(
            EasyHarvestingClient.Settings.SpeedMultiplier,
            EasyHarvestingServer.Settings.SpeedMultiplier);
    }
}