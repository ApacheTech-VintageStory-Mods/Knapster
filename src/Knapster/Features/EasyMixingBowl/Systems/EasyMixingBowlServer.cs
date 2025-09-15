namespace Knapster.Features.EasyMixingBowl.Systems;

public class EasyMixingBowlServer : EasyXServerSystemBase<EasyMixingBowlServer, EasyMixingBowlServerSettings, EasyMixingBowlClientSettings>
{
    protected override string SubCommandName => "MixingBowl";

    public override bool ShouldLoad(ICoreAPI api)
    {
        // https://mods.vintagestory.at/aculinaryartillery
        if (!api.ModLoader.IsModEnabled("aculinaryartillery")) return false;
        return base.ShouldLoad(api);
    }

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.T("EasyMixingBowl", "Description"));

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
            .WithDescription(G.T("EasyMixingBowl.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("automated")
            .WithAlias("a")
            .WithArgs(parsers.Bool("enabled"))
            .WithDescription(G.T("EasyMixingBowl.IncludeAutomated", "Description"))
            .HandleWith(OnChangeIncludeAutomated)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.T("EasyMixingBowl", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton));
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
        var message = G.T("EasyMixingBowl", "StickyMouseButton", SubCommandName, Settings.StickyMouseButton);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 10f);
        var message = G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeIncludeAutomated(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.IncludeAutomated = value;
        var message = G.T("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}