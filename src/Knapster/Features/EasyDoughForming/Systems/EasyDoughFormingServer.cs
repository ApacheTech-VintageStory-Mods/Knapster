using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Settings;
using Gantry.Core.Extensions.Api;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyDoughForming.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyDoughFormingServer : EasyXServerSystemBase<EasyDoughFormingServerSettings, EasyDoughFormingClientSettings, EasyDoughFormingSettings>
{
    protected override string SubCommandName => "DoughForming";

    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api) 
        && api.ModLoader.AreAllModsLoaded("artofcooking", "coreofarts");

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyDoughForming", "Description"));

        subCommand
            .BeginSubCommand("voxels")
            .WithAlias("v")
            .WithArgs(parsers.OptionalInt("voxels"))
            .WithDescription(LangEx.FeatureString("EasyDoughForming.VoxelsPerClick", "Description"))
            .HandleWith(OnChangeVoxelsPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("instant")
            .WithAlias("i")
            .WithArgs(parsers.OptionalBool("instant complete"))
            .WithDescription(LangEx.FeatureString("EasyDoughForming.InstantComplete", "Description"))
            .HandleWith(OnChangeInstantComplete)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
        sb.AppendLine(LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
    }

    private TextCommandResult OnChangeVoxelsPerClick(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.VoxelsPerClick = GameMath.Clamp(value, 1, 8);
        var message = LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeInstantComplete(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.InstantComplete = value;
        var message = LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}