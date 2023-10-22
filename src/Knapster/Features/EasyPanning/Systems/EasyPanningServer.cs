using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Systems;

[UsedImplicitly]
public sealed class EasyPanningServer : FeatureServerSystemBase<EasyPanningSettings, EasyPanningPacket>
{
    protected override string SubCommandName => "Panning";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyPanning", "Description"));
        
        subCommand
            .BeginSubCommand("speed")
            .WithAlias("sp")
            .WithArgs(parsers.OptionalFloat("speed"))
            .WithDescription(LangEx.FeatureString("EasyPanning.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("saturation")
            .WithAlias("sa")
            .WithArgs(parsers.OptionalFloat("rate"))
            .WithDescription(LangEx.FeatureString("EasyPanning.SaturationMultiplier", "Description"))
            .HandleWith(OnChangeSaturationMultiplier)
            .EndSubCommand();
    }

    protected override EasyPanningPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
    {
        return EasyPanningPacket.Create(enabledForPlayer, Settings.SpeedMultiplier, Settings.SaturationMultiplier);
    }

    protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
        sb.AppendLine(LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
        sb.AppendLine(LangEx.FeatureString("Knapster", "SaturationMultiplier", SubCommandName, Settings.SaturationMultiplier));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 2f);
        var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeSaturationMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SaturationMultiplier = GameMath.Clamp(value, 0f, 2f);
        var message = LangEx.FeatureString("Knapster", "SaturationMultiplier", SubCommandName, Settings.SaturationMultiplier);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}