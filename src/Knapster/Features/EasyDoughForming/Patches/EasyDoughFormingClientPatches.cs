using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Patches;

[HarmonyClientSidePatch]
[RequiresMod("coreofarts")]
[RequiresMod("artofcooking")]
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyDoughFormingClientPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("ArtOfCooking.Items.AOCItemDough", "GetToolModes")]
    public static void ClientPatch_AOCItemDough_GetToolModes_Postfix(Item __instance, ItemSlot slot,
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
    [HarmonyPatch("ArtOfCooking.BlockEntities.BlockEntityDoughForm", "GetBlockInfo")]
    public static void ClientPatch_BlockEntityDoughForm_GetBlockInfo_Postfix(dynamic __instance, StringBuilder dsc)
    {
        bool[,,] voxels = __instance.Voxels;
        bool[,,]? recipeVoxels = __instance.SelectedRecipe?.Voxels;
        if (recipeVoxels is null) return;

        var lengthX = voxels.GetLength(0);
        var lengthY = voxels.GetLength(1);
        var lengthZ = voxels.GetLength(2);

        var indices = from y in Enumerable.Range(0, lengthY)
                      from x in Enumerable.Range(0, lengthX)
                      from z in Enumerable.Range(0, lengthZ)
                      select new { x, y, z };

        var voxelsThatNeedFilling = indices.Count(idx => recipeVoxels[idx.x, idx.y, idx.z] && !voxels[idx.x, idx.y, idx.z]);
        var voxelsThatNeedRemoving = indices.Count(idx => !recipeVoxels[idx.x, idx.y, idx.z] && voxels[idx.x, idx.y, idx.z]);

        var totalDoughCost = (voxelsThatNeedFilling - voxelsThatNeedRemoving) / 36;
        if (totalDoughCost == -1) return;
        dsc.AppendLine(LangEx.FeatureString("EasyDoughForming", "DoughRequired", totalDoughCost));
    }
}