using Knapster.Features.EasySmithing.DataStructures;
using System.Diagnostics;

namespace Knapster.Features.EasySmithing.Patches;

[HarmonyUniversalPatch]
public partial class EasySmithingUniversalPatches
{
    [HarmonyPrefix]
    [HarmonyUniversalPatch(typeof(BlockEntityAnvil), "OnUseOver", typeof(IPlayer), typeof(Vec3i), typeof(BlockSelection))]
    public static bool UniversalPatch_BlockEntityAnvil_OnUseOver_Prefix(BlockEntityAnvil __instance,
        IPlayer byPlayer, Vec3i voxelPos, BlockSelection blockSel)
    {
        if (__instance.SelectedRecipe?.Voxels is null) return true;
        try
        {
            var slot = byPlayer.InventoryManager.ActiveHotbarSlot;
            if (slot.Itemstack is null || !__instance.CanWorkCurrent) return true;
            if (slot.Itemstack.Collectible is not ItemHammer hammer) return true;
            var toolMode = hammer.GetToolMode(slot, byPlayer, blockSel);
            var settings = new GetSmithingSettingsCommand(byPlayer);
            G.CommandProcessor.Send(settings);
            if (!settings.Enabled)
            {
                if (toolMode > 5) hammer.SetToolMode(slot, byPlayer, blockSel, 0);
                return true;
            }
            if (toolMode < 6) return true;

            // ----

            if (__instance.Api.Side.IsClient())
            {
                __instance.CallMethod("SendUseOverPacket", byPlayer, voxelPos);
            }
            OnHit(__instance, settings.InstantComplete ? 999 : settings.VoxelsPerClick, byPlayer);

            __instance.CallMethod("RegenMeshAndSelectionBoxes");
            slot.Itemstack.Collectible.DamageItem(__instance.Api.World, byPlayer.Entity, slot, settings.CostPerClick);
            if (!__instance.CallMethod<bool>("HasAnyMetalVoxel"))
            {
                __instance.CallMethod("clearWorkSpace");
                return false;
            }
            __instance.CheckIfFinished(byPlayer);
            __instance.MarkDirty();
            return false;
        }
        catch (ArgumentNullException ex)
        {
            G.Logger.Error(ex);
            return true;
        }
    }

    internal static void OnHit(BlockEntityAnvil anvil, int iterations, IPlayer byPlayer)
    {
        while (iterations-- > 0)
        {
            try
            {
                var result = ProcessHit(anvil, byPlayer);
                if (result.Action 
                    is AnvilHitAction.Nothing 
                    or AnvilHitAction.ItemCompleted)
                    break;
            }
            catch (Exception ex)
            {
                anvil.Api.Logger.Error(ex);
            }
        }
    }

    public static AnvilHitResult ProcessHit(BlockEntityAnvil anvil, IPlayer byPlayer)
    {
        if (anvil.SelectedRecipe is null)
        {
            return new(AnvilHitAction.Nothing);
        }
        anvil.CheckIfFinished(byPlayer);
        if (anvil.CallMethod<bool>("MatchesRecipe"))
        {
            return new(AnvilHitAction.ItemCompleted);
        }

        anvil.rotation = 0;
        var recipe = anvil.SelectedRecipe;
        var yMax = recipe.QuantityLayers;
        var usableMetalVoxel = anvil.CallMethod<Vec3i>("findFreeMetalVoxel");
        for (var x = 0; x < 16; x++)
            for (var z = 0; z < 16; z++)
                for (var y = 0; y < 6; y++)
                {
                    var requireMetalHere = y < yMax && recipe.Voxels[x, y, z];
                    var mat = (EnumVoxelMaterial)anvil.Voxels[x, y, z];
                    if (mat == EnumVoxelMaterial.Slag)
                    {
                        return ProcessRemoveSlag(anvil, byPlayer, x, z, y);
                    }
                    if (!requireMetalHere || usableMetalVoxel is null || mat != EnumVoxelMaterial.Empty) continue;
                    return ProcessMove(anvil, byPlayer, usableMetalVoxel, x, z, y, mat);
                }

        return usableMetalVoxel is null
            ? new(AnvilHitAction.ItemCompleted)
            : ProcessSplit(anvil, byPlayer, usableMetalVoxel);
    }

    internal static AnvilHitResult ProcessMove(BlockEntityAnvil anvil, IPlayer byPlayer, Vec3i usableMetalVoxel, int x, int z, int y, EnumVoxelMaterial mat)
    {
        anvil.Voxels[x, y, z] = 1;
        anvil.Voxels[usableMetalVoxel.X, usableMetalVoxel.Y, usableMetalVoxel.Z] = 0;
        OnHitSuccess(anvil, mat, usableMetalVoxel, x, y, z);

        var moves = Math.Abs(usableMetalVoxel.X - x) + Math.Abs(usableMetalVoxel.Z - z);
        return new(AnvilHitAction.MetalMoved, byPlayer, VoxelPos: new(x, y, z), Moves: moves);
    }

    internal static AnvilHitResult ProcessRemoveSlag(BlockEntityAnvil anvil, IPlayer byPlayer, int x, int z, int y)
    {
        var usableMetalVoxel = new Vec3i(x, y, z);
        AnvilMetalRecovery("Prefix_OnSplit", usableMetalVoxel, anvil);
        anvil.Voxels[x, y, z] = 0;
        OnHitSuccess(anvil, EnumVoxelMaterial.Slag, null, x, y, z);
        AnvilMetalRecovery("Postfix_OnSplit", usableMetalVoxel, anvil);
        SmithingPlusBitRecovery(anvil, byPlayer);
        return new(AnvilHitAction.SlagRemoved, byPlayer, VoxelPos: new(x, y, z));
    }

    internal static AnvilHitResult ProcessSplit(BlockEntityAnvil anvil, IPlayer byPlayer, Vec3i usableMetalVoxel)
    {
        AnvilMetalRecovery("Prefix_OnSplit", usableMetalVoxel, anvil);
        anvil.Voxels[usableMetalVoxel.X, usableMetalVoxel.Y, usableMetalVoxel.Z] = 0;
        OnHitSuccess(anvil, EnumVoxelMaterial.Metal, null, usableMetalVoxel.X, usableMetalVoxel.Y, usableMetalVoxel.Z);
        AnvilMetalRecovery("Postfix_OnSplit", usableMetalVoxel, anvil);
        SmithingPlusBitRecovery(anvil, byPlayer);
        return new(AnvilHitAction.MetalSplit, byPlayer, VoxelPos: usableMetalVoxel);
    }

    internal static void AnvilMetalRecovery(string methodName, Vec3i voxelPos, BlockEntityAnvil anvil)
    {
        if (!G.Uapi.ModLoader.AreAnyModsLoaded("metalrecovery", "metalrecoveryrevived", "anvilmetalrecoveryrevived")) return;
        var type = AccessTools.TypeByName("AnvilMetalRecovery.Patches.AnvilDaptor");
        if (type is null) return;
        var method = AccessTools.Method(type, methodName);
        method?.Invoke(null, [voxelPos, anvil]);
    }

    internal static void SmithingPlusBitRecovery(BlockEntityAnvil anvil, IPlayer byPlayer)
    {
        if (!G.Uapi.ModLoader.AreAnyModsLoaded("smithingplus")) return;
        if (anvil.WorkItemStack is not { } workItemStack) return;
        var type = AccessTools.TypeByName("SmithingPlus.BitsRecovery.BitsRecoveryPatches");
        if (type is null) return;
        var method = AccessTools.Method(type, "RecoverBitsFromWorkItem");
        method?.Invoke(null, [anvil, byPlayer, workItemStack]);
    }

    internal static void OnHitSuccess(BlockEntityAnvil anvil, EnumVoxelMaterial mat, Vec3i? usableMetalVoxel, int x, int y, int z)
    {
        if (anvil.Api.World.Side == EnumAppSide.Client)
        {
            anvil.CallMethod("spawnParticles", new Vec3i(x, y, z), (mat == EnumVoxelMaterial.Empty) ? EnumVoxelMaterial.Metal : mat, null!);
            if (usableMetalVoxel is not null) anvil.CallMethod("spawnParticles", usableMetalVoxel, EnumVoxelMaterial.Metal, null!);
        }
        anvil.CallMethod("RegenMeshAndSelectionBoxes");
        anvil.CheckIfFinished(null);
    }
}