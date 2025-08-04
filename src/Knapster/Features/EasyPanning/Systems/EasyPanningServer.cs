using Knapster.Features.EasyPanning.Settings;

namespace Knapster.Features.EasyPanning.Systems;

public sealed class EasyPanningServer : EasyXServerSystemBase<EasyPanningServer, EasyPanningServerSettings, EasyPanningClientSettings, EasyPanningSettings>
{
    protected override string SubCommandName => "Panning";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(G.Lang.FeatureString("EasyPanning", "Description"));
        
        subCommand
            .BeginSubCommand("time")
            .WithAlias("t")
            .WithArgs(parsers.OptionalFloat("time in seconds"))
            .WithDescription(G.Lang.FeatureString("EasyPanning.SecondsPerLayer", "Description"))
            .HandleWith(OnChangeSecondsPerLayer)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("drops")
            .WithAlias("d")
            .WithArgs(parsers.OptionalInt("drops per layer"))
            .WithDescription(G.Lang.FeatureString("EasyPanning.DropsPerLayer", "Description"))
            .HandleWith(OnChangeDropsPerLayer)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("saturation")
            .WithAlias("s")
            .WithArgs(parsers.OptionalFloat("saturation per layer"))
            .WithDescription(G.Lang.FeatureString("EasyPanning.SaturationPerLayer", "Description"))
            .HandleWith(OnChangeSaturationPerLayer)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(G.Lang.FeatureString("EasyPanning", "SecondsPerLayer", SubCommandName, Settings.SecondsPerLayer));
        sb.AppendLine(G.Lang.FeatureString("EasyPanning", "DropsPerLayer", SubCommandName, Settings.DropsPerLayer));
        sb.AppendLine(G.Lang.FeatureString("EasyPanning", "SaturationPerLayer", SubCommandName, Settings.SaturationPerLayer));
    }

    private TextCommandResult OnChangeSecondsPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 4f;
        Settings.SecondsPerLayer = GameMath.Clamp(value, 0f, 10f);

        var message = G.Lang.FeatureString("EasyPanning", "SecondsPerLayer", SubCommandName, Settings.SecondsPerLayer);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeDropsPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.DropsPerLayer = GameMath.Clamp(value, 0, 10);

        var message = G.Lang.FeatureString("EasyPanning", "DropsPerLayer", SubCommandName, Settings.DropsPerLayer);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeSaturationPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 3f;
        Settings.SaturationPerLayer = GameMath.Clamp(value, 0f, 10f);

        var message = G.Lang.FeatureString("EasyPanning", "SaturationPerLayer", SubCommandName, Settings.SaturationPerLayer);
        ServerChannel?.BroadcastUniquePacket(Sapi.AsServerMain(), GeneratePacket);
        return TextCommandResult.Success(message);
    }
}