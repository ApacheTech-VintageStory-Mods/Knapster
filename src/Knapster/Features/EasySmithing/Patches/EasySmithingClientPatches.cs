using ApacheTech.VintageMods.Knapster.Features.EasySmithing.Systems;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EasySmithingClientPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemHammer), nameof(ItemHammer.GetToolModes))]
        public static void ClientPatch_ItemHammer_GetToolModes_Postfix(ItemHammer __instance, ItemSlot slot,
            IClientPlayer forPlayer, BlockSelection blockSel, ref SkillItem[] __result, ref SkillItem[] ___toolModes)
        {
            if (__result is null) return;
            if (!EasySmithingClient.Settings.Enabled)
            {
                __result = ___toolModes = ___toolModes.Take(6).ToArray();
                if (__instance.GetToolMode(slot, forPlayer, blockSel) < 6) return;
                __instance.SetToolMode(slot, forPlayer, blockSel, 0);
                return;
            }

            if (___toolModes.Length > 6) return;
            var skillItem = new SkillItem
            {
                Code = new AssetLocation("auto"),
                Name = LangEx.FeatureString("Knapster", "AutoComplete")
            }.WithIcon(ApiEx.Client, ApiEx.Client.Gui.Icons.Drawfloodfill_svg);
            __result = ___toolModes = ___toolModes.AddToArray(skillItem);
        }
    }
}