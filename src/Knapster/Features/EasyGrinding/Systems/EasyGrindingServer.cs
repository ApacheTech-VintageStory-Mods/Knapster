namespace Knapster.Features.EasyGrinding.Systems;

/// <summary>
///     Server-side system for the EasyGrinding feature, providing chat commands and settings management for quern automation and speed.
/// </summary>
public class EasyGrindingServer : EasyXServerSystemBase<EasyGrindingServer, EasyGrindingServerSettings, EasyGrindingClientSettings>
{
    /// <summary>
    ///     Gets the sub-command name for this feature.
    /// </summary>
    protected override string SubCommandName => "Grinding";

    /// <summary>
    ///     Adds feature-specific sub-commands to the EasyGrinding command.
    /// </summary>
    /// <param name="subCommand">The sub-command to add features to.</param>
    /// <param name="parsers">The collection of parsers that can be used to parse command arguments.</param>
    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.T("EasyGrinding", "Description"));

        subCommand
            .BeginSubCommand("hotkey")
            .WithAlias("h")
            .WithArgs(parsers.Bool("sticky keys"))
            .WithDescription(G.T("EasyGrinding.StickyMouseButton", "Description"))
            .HandleWith(OnChangeStickyMouseButton)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.Float("multiplier"))
            .WithDescription(G.T("EasyGrinding.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("automated")
            .WithAlias("a")
            .WithArgs(parsers.Bool("enabled"))
            .WithDescription(G.T("EasyGrinding.IncludeAutomated", "Description"))
            .HandleWith(OnChangeIncludeAutomated)
            .EndSubCommand();
    }

    /// <summary>
    ///     Appends extra display information about the EasyGrinding feature to the provided StringBuilder.
    /// </summary>
    /// <param name="sb">The StringBuilder to append information to.</param>
    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.T("EasyGrinding", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton));
        sb.AppendLine(G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
        sb.AppendLine(G.T("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated));
    }

    /// <summary>
    ///     Handles the chat command to change the sticky mouse button setting for the quern.
    /// </summary>
    /// <param name="args">The command arguments.</param>
    /// <returns>A <see cref="TextCommandResult"/> indicating the result of the operation.</returns>
    private TextCommandResult OnChangeStickyMouseButton(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.StickyMouseButton = value;
        var message = G.T("EasyGrinding", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    /// <summary>
    ///     Handles the chat command to change the speed multiplier for the quern.
    /// </summary>
    /// <param name="args">The command arguments.</param>
    /// <returns>A <see cref="TextCommandResult"/> indicating the result of the operation.</returns>
    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 10f);
        var message = G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    /// <summary>
    ///     Handles the chat command to change whether automated querns are included in the speed multiplier.
    /// </summary>
    /// <param name="args">The command arguments.</param>
    /// <returns>A <see cref="TextCommandResult"/> indicating the result of the operation.</returns>
    private TextCommandResult OnChangeIncludeAutomated(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.IncludeAutomated = value;
        var message = G.T("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}