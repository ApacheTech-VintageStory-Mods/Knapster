namespace Knapster.Features.EasyMixingBowl.Systems;

public class EasyMixingBowlServer : EasyXServerSystemBase<EasyMixingBowlServer, EasyMixingBowlServerSettings, EasyMixingBowlClientSettings, EasyMixingBowlSettings>
{
    protected override string SubCommandName => "MixingBowl";

    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.IsModEnabled("aculinaryartillery");

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.Lang.FeatureString("EasyMixingBowl", "Description"));

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.Float("multiplier"))
            .WithDescription(G.Lang.FeatureString("EasyMixingBowl.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("automated")
            .WithAlias("a")
            .WithArgs(parsers.Bool("enabled"))
            .WithDescription(G.Lang.FeatureString("EasyMixingBowl.IncludeAutomated", "Description"))
            .HandleWith(OnChangeIncludeAutomated)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.Lang.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
        sb.AppendLine(G.Lang.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated));
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 10f);
        var message = G.Lang.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeIncludeAutomated(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.IncludeAutomated = value;
        var message = G.Lang.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}