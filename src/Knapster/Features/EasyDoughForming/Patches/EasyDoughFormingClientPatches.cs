using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Extensions;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;
using ArtOfCooking.BlockEntities;
using ArtOfCooking.Items;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Patches;

[HarmonyClientSidePatch]
[RequiresMod("coreofarts")]
[RequiresMod("artofcooking")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyDoughFormingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AOCItemDough), "GetToolModes")]
    public static void ClientPatch_AOCItemDough_GetToolModes_Postfix(AOCItemDough __instance, ItemSlot slot,
        IClientPlayer forPlayer, BlockSelection blockSel, ref SkillItem[] __result, ref SkillItem[] ___toolModes)
    {
        try
        {
            if (__result is null) return;
            if (!EasyDoughFormingClient.Settings.Enabled)
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
                Name = LangEx.FeatureString("Knapster", "AutoComplete")
            }.WithIcon(ApiEx.Client, ApiEx.Client.Gui.Icons.Drawfloodfill_svg);
            __result = ___toolModes = ___toolModes.AddToArray(skillItem);
        }
        catch (ArgumentNullException ex)
        {
            ModEx.Mod.Logger.Error(ex);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityDoughForm), "GetBlockInfo", MethodType.Normal)]
    public static void ClientPatch_BlockEntityDoughForm_GetBlockInfo_Postfix(BlockEntityDoughForm __instance, StringBuilder dsc)
    {
        var totalDoughCost = __instance.TotalDoughCost();
        if (totalDoughCost == -1) return;
        dsc.AppendLine(LangEx.FeatureString("EasyDoughForming", "DoughRequired", totalDoughCost));
    }
}