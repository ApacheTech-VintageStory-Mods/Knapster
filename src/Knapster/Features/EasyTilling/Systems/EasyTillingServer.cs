namespace Knapster.Features.EasyTilling.Systems;

public sealed class EasyTillingServer : EasyXServerSystemBase<EasyTillingServer, EasyTillingServerSettings, EasyTillingClientSettings>
{
    protected override string SubCommandName => "Tilling";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.T("EasyTilling", "Description"));

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.OptionalFloat("speed"))
            .WithDescription(G.T("EasyTilling", "SpeedMultiplier.Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.Append(G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0.1f, 2f);
        var message = G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}