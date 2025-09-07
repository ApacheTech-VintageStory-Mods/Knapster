namespace Knapster.Features.EasySmithing.Patches;

[HarmonyClientSidePatch]
public class EasySmithingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemHammer), nameof(ItemHammer.GetToolModes))]
    public static void ClientPatch_ItemHammer_GetToolModes_Postfix(ItemHammer __instance, ItemSlot slot,
        IClientPlayer forPlayer, BlockSelection blockSel, ref SkillItem[] __result, ref SkillItem[] ___toolModes)
    {
        var settings = new GetSmithingSettingsCommand(forPlayer);
        G.CommandProcessor.Send(settings);
        try
        {
            if (__result is null) return;
            if (!settings.Enabled)
            {
                __result = ___toolModes = [.. ___toolModes.Take(6)];
                if (__instance.GetToolMode(slot, forPlayer, blockSel) < 6) return;
                __instance.SetToolMode(slot, forPlayer, blockSel, 0);
                return;
            }

            if (___toolModes.Length > 6) return;
            var skillItem = new SkillItem
            {
                Code = new AssetLocation("auto"),
                Name = G.T("Knapster", "AutoComplete")
            }.WithIcon(G.Capi, G.Capi!.Gui.Icons.Drawfloodfill_svg);
            __result = ___toolModes = ___toolModes.AddToArray(skillItem);
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
        }
    }
}