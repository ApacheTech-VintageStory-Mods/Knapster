namespace Knapster.Features.EasyKnapping.Systems;

public sealed class EasyKnappingServer : EasyXServerSystemBase<EasyKnappingServer, EasyKnappingServerSettings, EasyKnappingClientSettings>
{
    protected override string SubCommandName => "Knapping";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.T("EasyKnapping", "Description"));

        subCommand
            .BeginSubCommand("voxels")
            .WithAlias("v")
            .WithArgs(parsers.OptionalInt("voxels"))
            .WithDescription(G.T("EasyKnapping.VoxelsPerClick", "Description"))
            .HandleWith(OnChangeVoxelsPerClick)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("instant")
            .WithAlias("i")
            .WithArgs(parsers.OptionalBool("instant complete"))
            .WithDescription(G.T("EasyKnapping.InstantComplete", "Description"))
            .HandleWith(OnChangeInstantComplete)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.T("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
        sb.AppendLine(G.T("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
    }

    private TextCommandResult OnChangeVoxelsPerClick(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.VoxelsPerClick = GameMath.Clamp(value, 1, 8);
        var message = G.T("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeInstantComplete(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.InstantComplete = value;
        var message = G.T("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}