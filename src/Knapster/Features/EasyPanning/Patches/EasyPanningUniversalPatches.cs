using System.Reflection.Emit;

namespace Knapster.Features.EasyPanning.Patches;

[HarmonyUniversalPatch]
public sealed class EasyPanningUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyUniversalPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStop))]
    public static bool Harmony_BlockPan_OnHeldInteractStop_Prefix(
        BlockPan __instance,
        float secondsUsed,
        ItemSlot slot,
        EntityAgent byEntity,
        ICoreAPI ___api,
        ref ILoadedSound? ___sound
    )
    {
        ___sound?.Stop();
        ___sound = null;

        if (!(secondsUsed >= SecondsPerLayer(byEntity) * 0.85)) return false;

        var code = __instance.GetBlockMaterialCode(slot.Itemstack);
        if (___api.Side.IsServer() && code is not null)
        {
            var drops = DropsPerLayer(byEntity);
            do __instance.CallMethod("CreateDrop", byEntity, code);
            while (--drops > 0);
        }
        __instance.RemoveMaterial(slot);
        slot.MarkDirty();
        var behaviour = byEntity.GetBehavior<EntityBehaviorHunger>();
        var saturation = SaturationPerLayer(byEntity);
        behaviour?.ConsumeSaturation(saturation);
        return false;
    }

    [HarmonyTranspiler]
    [HarmonyUniversalPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStep))]
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
    {
        if (byEntity is not EntityPlayer playerEntity) return 4f;

        if (!G.ApiEx.Return(
                () => EasyPanningClient.Instance.Settings.Enabled,
                () => EasyPanningServer.Instance.IsEnabledFor(playerEntity.Player)))
        {
            return 4f;
        }

        return G.ApiEx.Return(
            () => EasyPanningClient.Instance.Settings.SecondsPerLayer,
            () => EasyPanningServer.Instance.Settings.SecondsPerLayer);
    }

    private static int DropsPerLayer(EntityAgent byEntity)
    {
        if (byEntity is not EntityPlayer playerEntity) return 1;

        if (!G.ApiEx.Return(
                () => EasyPanningClient.Instance.Settings.Enabled,
                () => EasyPanningServer.Instance.IsEnabledFor(playerEntity.Player)))
        {
            return 1;
        }

        return G.ApiEx.Return(
            () => EasyPanningClient.Instance.Settings.DropsPerLayer,
            () => EasyPanningServer.Instance.Settings.DropsPerLayer);
    }

    private static float SaturationPerLayer(EntityAgent byEntity)
    {
        if (byEntity is not EntityPlayer playerEntity) return 3f;

        if (!G.ApiEx.Return(
                () => EasyPanningClient.Instance.Settings.Enabled,
                () => EasyPanningServer.Instance.IsEnabledFor(playerEntity.Player)))
        {
            return 3f;
        }

        return G.ApiEx.Return(
            () => EasyPanningClient.Instance.Settings.SaturationPerLayer,
            () => EasyPanningServer.Instance.Settings.SaturationPerLayer);
    }
}