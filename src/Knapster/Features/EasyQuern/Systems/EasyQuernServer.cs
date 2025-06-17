using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Systems;

/// <summary>
///     Server-side system for the EasyQuern feature, providing chat commands and settings management for quern automation and speed.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyQuernServer : EasyXServerSystemBase<EasyQuernServerSettings, EasyQuernClientSettings, EasyQuernSettings>
{
    /// <summary>
    ///     Gets the sub-command name for this feature.
    /// </summary>
    protected override string SubCommandName => "Quern";

    /// <summary>
    ///     Adds feature-specific sub-commands to the EasyQuern command.
    /// </summary>
    /// <param name="subCommand">The sub-command to add features to.</param>
    /// <param name="parsers">The collection of parsers that can be used to parse command arguments.</param>
    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyQuern", "Description"));

        subCommand
            .BeginSubCommand("hotkey")
            .WithAlias("h")
            .WithArgs(parsers.Bool("sticky keys"))
            .WithDescription(LangEx.FeatureString("EasyQuern.StickyMouseButton", "Description"))
            .HandleWith(OnChangeStickyMouseButton)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.Float("multiplier"))
            .WithDescription(LangEx.FeatureString("EasyQuern.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("automated")
            .WithAlias("a")
            .WithArgs(parsers.Bool("enabled"))
            .WithDescription(LangEx.FeatureString("EasyQuern.IncludeAutomated", "Description"))
            .HandleWith(OnChangeIncludeAutomated)
            .EndSubCommand();
    }

    /// <summary>
    ///     Appends extra display information about the EasyQuern feature to the provided StringBuilder.
    /// </summary>
    /// <param name="sb">The StringBuilder to append information to.</param>
    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(LangEx.FeatureString("EasyQuern", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton));
        sb.AppendLine(LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
        sb.AppendLine(LangEx.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated));
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
        var message = LangEx.FeatureString("EasyQuern", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
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
        var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
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
        var message = LangEx.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}