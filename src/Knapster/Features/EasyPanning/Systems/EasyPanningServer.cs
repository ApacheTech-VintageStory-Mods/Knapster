using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Systems
{
    [UsedImplicitly]
    public sealed class EasyPanningServer : FeatureServerSystemBase<EasyPanningSettings, EasyPanningPacket>
    {
        protected override string SubCommandName => "Panning";

        protected override void FeatureSpecificCommands(IChatCommand subCommand)
        {
            // ReSharper disable StringLiteralTypo

            subCommand
                .BeginSubCommand("speed")
                .WithAlias("s")
                .WithArgs(new WordArgParser("speed", false))
                .HandleWith(OnChangeSpeedMultiplier)
                .EndSubCommand();

            subCommand
                .BeginSubCommand("hungerrate")
                .WithAlias("h")
                .WithArgs(new WordArgParser("rate", false))
                .HandleWith(OnChangeSaturationMultiplier)
                .EndSubCommand();

            // ReSharper enable StringLiteralTypo
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
            sb.Append(LangEx.FeatureString("Knapster", "SaturationMultiplier", SubCommandName, Settings.SaturationMultiplier));
            return TextCommandResult.Success(sb.ToString());
        }

        private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
        {
            Settings.SpeedMultiplier = GameMath.Clamp(args.RawArgs.PopFloat().GetValueOrDefault(1f), 0f, 2f);
            var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }

        private TextCommandResult OnChangeSaturationMultiplier(TextCommandCallingArgs args)
        {
            Settings.SaturationMultiplier = GameMath.Clamp(args.RawArgs.PopFloat().GetValueOrDefault(1f), 0f, 2f);
            var message = LangEx.FeatureString("Knapster", "SaturationMultiplier", SubCommandName, Settings.SaturationMultiplier);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }
    }
}
