using System.Reflection.Emit;

namespace Knapster.Features.EasyPanning.Patches;

[HarmonyUniversalPatch]
public sealed class EasyPanningUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStop))]
    public static bool Harmony_BlockPan_OnHeldInteractStop_Prefix(
        BlockPan __instance,
        float secondsUsed,
        ItemSlot slot,
        EntityAgent byEntity
    )
    {
        if (byEntity is not EntityPlayer player) return true;

        var sound = __instance.GetField<ILoadedSound>("sound");
        sound?.Stop();
        __instance.SetField("sound", null!);

        if (!(secondsUsed >= SecondsPerLayer(player) * 0.85)) return false;

        var code = __instance.GetBlockMaterialCode(slot.Itemstack);
        if (G.Side.IsServer() && code is not null)
        {
            var drops = DropsPerLayer(player);
            for (var i = 0; i < drops; i++) __instance.CallMethod("CreateDrop", player, code);
        }
        __instance.RemoveMaterial(slot);
        slot.MarkDirty();
        var behaviour = player.GetBehavior<EntityBehaviorHunger>();
        var saturation = SaturationPerLayer(player);
        behaviour?.ConsumeSaturation(saturation);
        return false;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStep))]
    public static IEnumerable<CodeInstruction> Harmony_BlockPan_OnHeldInteractStep_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var result = new List<CodeInstruction>();
        var codeInstructions = instructions.ToArray();

        for (var i = 0; i < codeInstructions.Length - 1; i++)
        {
            var current = codeInstructions[i];
            var next = codeInstructions[i + 1];


            if (!(current.Is(OpCodes.Ldc_R4, 4f) && next.opcode == OpCodes.Cgt_Un))
            {
                result.Add(current);
            }
            else
            {
                result.Add(new CodeInstruction(OpCodes.Ldarg_3));
                result.Add(CodeInstruction.Call(typeof(EasyPanningUniversalPatches), nameof(SecondsPerLayer), [typeof(EntityAgent)]));
            }
        }

        result.Add(codeInstructions.Last());
        return result;
    }

    private static float SecondsPerLayer(EntityAgent byEntity)
        => G.CommandProcessor.Handle(new GetPanningSecondsPerLayerCommand(byEntity)).SecondsPerLayer;

    private static int DropsPerLayer(EntityAgent byEntity)
        => G.CommandProcessor.Handle(new GetPanningDropsPerLayerCommand(byEntity)).DropsPerLayer;

    private static float SaturationPerLayer(EntityAgent byEntity)
        => G.CommandProcessor.Handle(new GetPanningSaturationPerLayerCommand(byEntity)).SaturationPerLayer;
}