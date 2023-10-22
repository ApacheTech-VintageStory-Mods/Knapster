using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Systems
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EasyClayFormingServer : FeatureServerSystemBase<EasyClayFormingSettings, EasyClayFormingPacket>
    {
        protected override string SubCommandName => "ClayForming";

        protected override void FeatureSpecificCommands(IChatCommand subCommand)
        {
            subCommand
                .BeginSubCommand("voxels")
                .WithAlias("v")
                .WithArgs(new WordArgParser("voxels", false))
                .HandleWith(OnChangeVoxelsPerClick)
                .EndSubCommand();

            subCommand
                .BeginSubCommand("instant")
                .WithAlias("i")
                .WithArgs(new WordArgParser("instant", false))
                .HandleWith(OnChangeInstantComplete)
                .EndSubCommand();
        }

        protected sealed override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
            sb.AppendLine(LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
            sb.AppendLine(LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
            return TextCommandResult.Success(sb.ToString());
        }

        protected override EasyClayFormingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
        {
            return EasyClayFormingPacket.Create(
                enabledForPlayer, 
                Settings.VoxelsPerClick, 
                Settings.InstantComplete);
        }

        private TextCommandResult OnChangeVoxelsPerClick(TextCommandCallingArgs args)
        {
            Settings.VoxelsPerClick = GameMath.Clamp(args.RawArgs.PopInt().GetValueOrDefault(1), 1, 8);
            var message = LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }

        private TextCommandResult OnChangeInstantComplete(TextCommandCallingArgs args)
        {
            Settings.InstantComplete = args.RawArgs.PopBool().GetValueOrDefault(false);
            var message = LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }
    }
}