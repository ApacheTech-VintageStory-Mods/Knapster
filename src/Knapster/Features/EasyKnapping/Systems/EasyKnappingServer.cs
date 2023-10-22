using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping.Systems
{
    [UsedImplicitly]
    public sealed class EasyKnappingServer : FeatureServerSystemBase<EasyKnappingSettings, EasyKnappingPacket>
    {
        protected override string SubCommandName => "Knapping";

        protected override void FeatureSpecificCommands(IChatCommand subCommand)
        {
            subCommand
                .BeginSubCommand("voxels")
                .WithAlias("v")
                .WithArgs(new WordArgParser("voxels", false))
                .HandleWith(OnChangeVoxelsPerClick)
                .EndSubCommand();
        }

        protected override EasyKnappingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
        {
            return EasyKnappingPacket.Create(enabledForPlayer, Settings.VoxelsPerClick);
        }

        protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
            sb.Append(LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
            return TextCommandResult.Success(sb.ToString());
        }

        private TextCommandResult OnChangeVoxelsPerClick(TextCommandCallingArgs args)
        {
            Settings.VoxelsPerClick = GameMath.Clamp(args.RawArgs.PopInt().GetValueOrDefault(1), 1, 8);
            var message = LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }
    }
}
