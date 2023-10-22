using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Systems
{
    [UsedImplicitly]
    public sealed class EasyHarvestingServer : FeatureServerSystemBase<EasyHarvestingSettings, EasyHarvestingPacket>
    {
        protected override string SubCommandName => "Harvesting";

        protected override void FeatureSpecificCommands(IChatCommand subCommand)
        {
            subCommand
                .BeginSubCommand("speed")
                .WithAlias("s")
                .WithArgs(new WordArgParser("speed", false))
                .HandleWith(OnChangeSpeedMultiplier)
                .EndSubCommand();
        }

        protected override EasyHarvestingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
        {
            return EasyHarvestingPacket.Create(enabledForPlayer, Settings.SpeedMultiplier);
        }

        protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
            sb.Append(LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
            return TextCommandResult.Success(sb.ToString());
        }

        private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
        {
            Settings.SpeedMultiplier = GameMath.Clamp(args.RawArgs.PopFloat().GetValueOrDefault(1f), 0f, 2f);
            var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }
    }
}
