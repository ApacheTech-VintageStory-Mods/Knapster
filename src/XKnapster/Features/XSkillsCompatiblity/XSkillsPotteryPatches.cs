using Knapster.Features.EasyClayForming.Extensions;

namespace XKnapster.Features.XSkillsCompatiblity;

[HarmonyUniversalPatch]
public static class XSkillsPotteryPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityClayFormExtensions), nameof(BlockEntityClayFormExtensions.VoxelsPerClay))]
    public static void Harmony_BlockEntityClayFormExtensions_VoxelsPerClay_Postfix(ref int __result, IPlayer byPlayer)
    {
        if (XLeveling.Instance(G.Uapi).GetSkill("pottery") is not Pottery pottery) return;
        var playerSkill = byPlayer.Entity.GetBehavior<PlayerSkillSet>()?[pottery.Id];
        if (playerSkill is null) return;
        var playerAbility = playerSkill[pottery.ThriftId];
        if (playerAbility is null) return;
        __result = 25 + playerAbility.Value(0);
    }
}
