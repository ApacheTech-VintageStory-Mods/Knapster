using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Settings;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyClayFormingServer : EasyXServerSystemBase<EasyClayFormingServerSettings, EasyClayFormingClientSettings, IEasyClayFormingSettings>
{
    protected override string SubCommandName => "ClayForming";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyClayForming", "Description"));

        subCommand
            .BeginSubCommand("voxels")
            .WithAlias("v")
            .WithArgs(parsers.OptionalInt("voxels"))
            .WithDescription(LangEx.FeatureString("EasyClayForming.VoxelsPerClick", "Description"))
            .HandleWith(OnChangeVoxelsPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("instant")
            .WithAlias("i")
            .WithArgs(parsers.OptionalBool("instant complete"))
            .WithDescription(LangEx.FeatureString("EasyClayForming.InstantComplete", "Description"))
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