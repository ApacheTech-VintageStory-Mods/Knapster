using ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Extensions;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;

#pragma warning disable IDE1006 // Naming Styles
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Patches;

[HarmonyClientSidePatch]
[RequiresMod("coreofarts")]
[RequiresMod("artofcooking")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyDoughFormingClientPatches
{
    private const string BlockEntityDoughForm = "ArtOfCooking.BlockEntities.BlockEntityDoughForm";
    private const string AOCItemDough = "ArtOfCooking.Items.AOCItemDough";

    [HarmonyPostfix]
    [HarmonyPatch(AOCItemDough, "GetToolModes")]
    public static void ClientPatch_AOCItemDough_GetToolModes_Postfix(Item __instance, ItemSlot slot,
        IClientPlayer forPlayer, BlockSelection blockSel, ref SkillItem[] __result, ref SkillItem[] ___toolModes)
    {
        try
        {
            if (__result is null) return;
            if (!EasyDoughFormingClient.Settings.Enabled)
            {
                __result = ___toolModes = [.. ___toolModes.Take(4)];
                if (__instance.CallMethod<int>("GetToolMode", slot, forPlayer, blockSel) < 4) return;
                __instance.CallMethod("SetToolMode", slot, forPlayer, blockSel, 0);
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
    [HarmonyPatch(BlockEntityDoughForm, "GetBlockInfo", MethodType.Normal)]
    public static void ClientPatch_BlockEntityDoughForm_GetBlockInfo_Postfix(BlockEntity __instance, StringBuilder dsc)
    {
        var selectedRecipe = __instance.GetField<object>("selectedRecipe");
        var recipeVoxels = selectedRecipe?.GetField<bool[,,]>("Voxels");
        var progress = __instance.GetField<bool[,,]>("Voxels");
        var maxStackSize = 36;
        var totalCost = recipeVoxels.TotalMaterialCost(progress, maxStackSize);
        if (totalCost == -1) return;
        dsc.AppendLine(LangEx.FeatureString("EasyDoughForming", "DoughRequired", totalCost));
    }
}