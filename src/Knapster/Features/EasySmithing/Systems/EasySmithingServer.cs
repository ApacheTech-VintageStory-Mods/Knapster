namespace Knapster.Features.EasySmithing.Systems;

public class EasySmithingServer : EasyXServerSystemBase<EasySmithingServer, EasySmithingServerSettings, EasySmithingClientSettings, EasySmithingSettings>
{
    protected override string SubCommandName => "Smithing";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.Lang.FeatureString("EasySmithing", "Description"));

        subCommand
            .BeginSubCommand("voxels")
            .WithAlias("v")
            .WithArgs(parsers.OptionalInt("voxels"))
            .WithDescription(G.Lang.FeatureString("EasySmithing.VoxelsPerClick", "Description"))
            .HandleWith(OnChangeVoxelsPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("cost")
            .WithAlias("c")
            .WithArgs(parsers.OptionalInt("cost"))
            .WithDescription(G.Lang.FeatureString("EasySmithing.CostPerClick", "Description"))
            .HandleWith(OnChangeCostPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("instant")
            .WithAlias("i")
            .WithDescription(G.Lang.FeatureString("EasySmithing.InstantComplete", "Description"))
            .WithArgs(parsers.OptionalBool("instant complete"))
            .HandleWith(OnChangeInstantComplete)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.Lang.FeatureString("Knapster", "CostPerClick", SubCommandName, Settings.CostPerClick));
        sb.AppendLine(G.Lang.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
        sb.AppendLine(G.Lang.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
    }

    private TextCommandResult OnChangeCostPerClick(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.CostPerClick = GameMath.Clamp(value, 1, 10);
        var message = G.Lang.FeatureString("EasySmithing", "CostPerClick", SubCommandName, Settings.CostPerClick);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeVoxelsPerClick(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.VoxelsPerClick = GameMath.Clamp(value, 1, 8);
        var message = G.Lang.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeInstantComplete(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.InstantComplete = value;
        var message = G.Lang.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}