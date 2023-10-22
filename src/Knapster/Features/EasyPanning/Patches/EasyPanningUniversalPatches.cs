using System.Reflection.Emit;
using ApacheTech.VintageMods.Knapster.Features.EasyPanning.Systems;
using Vintagestory.API.Common.Entities;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Patches
{
    [HarmonySidedPatch(EnumAppSide.Universal)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class EasyPanningUniversalPatches
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStop))]
        public static IEnumerable<CodeInstruction> Harmony_BlockPan_OnHeldInteractStop_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var result = new List<CodeInstruction>();
            
            foreach (var codeInstruction in instructions)
            {
                result.Add(codeInstruction);

                if (codeInstruction.Is(OpCodes.Ldc_R4, 3.4f))
                {
                    result.Add(new CodeInstruction(OpCodes.Ldarg_3));
                    result.Add(CodeInstruction.Call(typeof(EasyPanningUniversalPatches), nameof(SpeedMultiplier), new []{ typeof(EntityAgent) }));
                    result.Add(new CodeInstruction(OpCodes.Mul));
                }

                if (codeInstruction.Is(OpCodes.Ldc_R4, 4f))
                {
                    result.Add(new CodeInstruction(OpCodes.Ldarg_3));
                    result.Add(CodeInstruction.Call(typeof(EasyPanningUniversalPatches), nameof(SaturationMultiplier), new[] { typeof(EntityAgent) }));
                    result.Add(new CodeInstruction(OpCodes.Mul));
                }
            }

            return result;
        }


        [HarmonyTranspiler]
        [HarmonyPatch(typeof(BlockPan), nameof(BlockPan.OnHeldInteractStep))]
        public static IEnumerable<CodeInstruction> Harmony_BlockPan_OnHeldInteractStep_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var result = new List<CodeInstruction>();
            var codeInstructions = instructions.ToArray();


            for (var i = 0; i < codeInstructions.Length -1; i++)
            {
                var current = codeInstructions[i];
                var next = codeInstructions[i+1];
                
                result.Add(current);

                if (!(current.Is(OpCodes.Ldc_R4, 4f) && next.opcode == OpCodes.Cgt_Un)) continue;
                result.Add(new CodeInstruction(OpCodes.Ldarg_3));
                result.Add(CodeInstruction.Call(typeof(EasyPanningUniversalPatches), nameof(SpeedMultiplier), new[] { typeof(EntityAgent) }));
                result.Add(new CodeInstruction(OpCodes.Mul));
            }

            result.Add(codeInstructions.Last());
            return result;
        }

        private void F(Entity entity)
        {
            if (entity is EntityPlayer player && 
                player.Player.WorldData.CurrentGameMode is EnumGameMode.Spectator) return;
        }

        private static float SpeedMultiplier(EntityAgent byEntity)
        {
            if (byEntity is not EntityPlayer playerEntity) return 1f;

            if (!ApiEx.Return(
                    () => EasyPanningClient.Settings.Enabled,
                    () => EasyPanningServer.IsEnabledFor(playerEntity.Player)))
            {
                return 1f;
            }

            return ApiEx.OneOf(
                EasyPanningClient.Settings.SpeedMultiplier, 
                EasyPanningServer.Settings.SpeedMultiplier);
        }

        private static float SaturationMultiplier(EntityAgent byEntity)
        {
            if (byEntity is not EntityPlayer playerEntity) return 1f;

            if (!ApiEx.Return(
                    () => EasyPanningClient.Settings.Enabled,
                    () => EasyPanningServer.IsEnabledFor(playerEntity.Player)))
            {
                return 1f;
            }

            return ApiEx.OneOf(
                EasyPanningClient.Settings.SaturationMultiplier,
                EasyPanningServer.Settings.SaturationMultiplier);
        }
    }
}