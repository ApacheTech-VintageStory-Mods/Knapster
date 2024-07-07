using ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Extensions;
using ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Systems;
using Vintagestory.API.Config;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Patches;

[HarmonySidedPatch(EnumAppSide.Client)]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
            if (!EasyClayFormingClient.Settings.Enabled)
            {
                __result = ___toolModes = ___toolModes.Take(4).ToArray();
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
    [HarmonyPatch(typeof(BlockEntityClayForm), nameof(BlockEntityClayForm.GetBlockInfo))]
    public static void ClientPatch_BlockEntityClayForm_GetBlockInfo_Postfix(BlockEntityClayForm __instance, StringBuilder dsc)
    {
        var totalClayCost = __instance.TotalClayCost();
        if (totalClayCost == -1) return;
        dsc.AppendLine(LangEx.FeatureString("EasyClayForming", "ClayRequired", totalClayCost));
    }
}