using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasySmithingServer : FeatureServerSystemBase<EasySmithingSettings, EasySmithingPacket>
{
    protected override string SubCommandName => "Smithing";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasySmithing", "Description"));

        subCommand
            .BeginSubCommand("voxels")
            .WithAlias("v")
            .WithArgs(parsers.OptionalInt("voxels"))
            .WithDescription(LangEx.FeatureString("EasySmithing.VoxelsPerClick", "Description"))
            .HandleWith(OnChangeVoxelsPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("cost")
            .WithAlias("c")
            .WithArgs(parsers.OptionalInt("cost"))
            .WithDescription(LangEx.FeatureString("EasySmithing.CostPerClick", "Description"))
            .HandleWith(OnChangeCostPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("instant")
            .WithAlias("i")
            .WithDescription(LangEx.FeatureString("EasySmithing.InstantComplete", "Description"))
            .WithArgs(parsers.OptionalBool("instant complete"))
            .HandleWith(OnChangeInstantComplete)
            .EndSubCommand();
    }

    protected override EasySmithingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
    {
        return EasySmithingPacket.Create(
            enabledForPlayer, 
            Settings.CostPerClick, 
            Settings.VoxelsPerClick, 
            Settings.InstantComplete);
    }

    protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
        sb.AppendLine(LangEx.FeatureString("Knapster", "CostPerClick", SubCommandName, Settings.CostPerClick));
        sb.AppendLine(LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
        sb.AppendLine(LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnChangeCostPerClick(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.CostPerClick = GameMath.Clamp(value, 1, 10);
        var message = LangEx.FeatureString("EasySmithing", "CostPerClick", SubCommandName, Settings.CostPerClick);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
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