using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Systems;

[UsedImplicitly]
public sealed class EasyPanningServer : EasyXServerSystemBase<EasyPanningServerSettings, EasyPanningClientSettings, EasyPanningSettings>
{
    protected override string SubCommandName => "Panning";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyPanning", "Description"));
        
        subCommand
            .BeginSubCommand("time")
            .WithAlias("t")
            .WithArgs(parsers.OptionalFloat("time in seconds"))
            .WithDescription(LangEx.FeatureString("EasyPanning.SecondsPerLayer", "Description"))
            .HandleWith(OnChangeSecondsPerLayer)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("drops")
            .WithAlias("d")
            .WithArgs(parsers.OptionalInt("drops per layer"))
            .WithDescription(LangEx.FeatureString("EasyPanning.DropsPerLayer", "Description"))
            .HandleWith(OnChangeDropsPerLayer)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("saturation")
            .WithAlias("s")
            .WithArgs(parsers.OptionalFloat("saturation per layer"))
            .WithDescription(LangEx.FeatureString("EasyPanning.SaturationPerLayer", "Description"))
            .HandleWith(OnChangeSaturationPerLayer)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(LangEx.FeatureString("EasyPanning", "SecondsPerLayer", SubCommandName, Settings.SecondsPerLayer));
        sb.AppendLine(LangEx.FeatureString("EasyPanning", "DropsPerLayer", SubCommandName, Settings.DropsPerLayer));
        sb.AppendLine(LangEx.FeatureString("EasyPanning", "SaturationPerLayer", SubCommandName, Settings.SaturationPerLayer));
    }

    private TextCommandResult OnChangeSecondsPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 4f;
        Settings.SecondsPerLayer = GameMath.Clamp(value, 0f, 10f);

        var message = LangEx.FeatureString("EasyPanning", "SecondsPerLayer", SubCommandName, Settings.SecondsPerLayer);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeDropsPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int?>() ?? 1;
        Settings.DropsPerLayer = GameMath.Clamp(value, 0, 10);

        var message = LangEx.FeatureString("EasyPanning", "DropsPerLayer", SubCommandName, Settings.DropsPerLayer);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeSaturationPerLayer(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 3f;
        Settings.SaturationPerLayer = GameMath.Clamp(value, 0f, 10f);

        var message = LangEx.FeatureString("EasyPanning", "SaturationPerLayer", SubCommandName, Settings.SaturationPerLayer);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}