using Knapster.Features.EasyClayForming.Extensions;
using Knapster.Features.EasyClayForming.Systems;


namespace Knapster.Features.EasyClayForming.Patches;

[HarmonyClientSidePatch]
public class EasyClayFormingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemClay), nameof(ItemClay.GetToolModes))]
    public static void ClientPatch_ItemClay_GetToolModes_Postfix(ItemClay __instance, ItemSlot slot,
        IClientPlayer forPlayer, BlockSelection blockSel, ref SkillItem[] __result, ref SkillItem[] ___toolModes)
    {
        try
        {
            if (__result is null) return;
            if (!EasyClayFormingClient.Instance.Settings.Enabled)
            {
                __result = ___toolModes = [.. ___toolModes.Take(4)];
                if (__instance.GetToolMode(slot, forPlayer, blockSel) < 4) return;
                __instance.SetToolMode(slot, forPlayer, blockSel, 0);
                return;
            }

            if (___toolModes.Length > 4) return;
            var skillItem = new SkillItem
            {
                Code = new AssetLocation("auto"),
                Name = G.Lang.FeatureString("Knapster", "AutoComplete")
            }.WithIcon(G.Capi, G.Capi.Gui.Icons.Drawfloodfill_svg);
            __result = ___toolModes = ___toolModes.AddToArray(skillItem);
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityClayForm), nameof(BlockEntityClayForm.GetBlockInfo))]
    public static void ClientPatch_BlockEntityClayForm_GetBlockInfo_Postfix(BlockEntityClayForm __instance, IPlayer forPlayer, StringBuilder dsc)
    {
        var totalClayCost = __instance.TotalClayCost(forPlayer);
        if (totalClayCost == -1) return;
        dsc.AppendLine(G.Lang.FeatureString("EasyClayForming", "ClayRequired", totalClayCost));
    }
}