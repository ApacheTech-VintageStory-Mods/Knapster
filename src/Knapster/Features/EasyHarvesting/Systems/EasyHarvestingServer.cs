namespace Knapster.Features.EasyHarvesting.Systems;

public sealed class EasyHarvestingServer : EasyXServerSystemBase<EasyHarvestingServer, EasyHarvestingServerSettings, EasyHarvestingClientSettings>
{
    protected override string SubCommandName => "Harvesting";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.T("EasyHarvesting", "Description"));

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.OptionalFloat("speed"))
            .WithDescription(G.T("EasyHarvesting.SpeedMultiplier", "Description"))
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
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 2f);
        var message = G.T("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}