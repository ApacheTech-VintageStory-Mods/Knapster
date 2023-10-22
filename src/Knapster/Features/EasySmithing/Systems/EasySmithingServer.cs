using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.DataStructures;
using ApacheTech.VintageMods.Knapster.Extensions;
using System.Globalization;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Systems
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EasySmithingServer : FeatureServerSystemBase<EasySmithingSettings, EasySmithingPacket>
    {
        protected override string SubCommandName => "Smithing";

        protected override void FeatureSpecificCommands(IChatCommand subCommand)
        {
            subCommand
                .BeginSubCommand("voxels")
                .WithAlias("v")
                .WithArgs(new WordArgParser("voxels", false))
                .HandleWith(OnChangeCostPerClick)
                .EndSubCommand();

            subCommand
                .BeginSubCommand("cost")
                .WithAlias("c")
                .WithArgs(new WordArgParser("cost", false))
                .HandleWith(OnChangeVoxelsPerClick)
                .EndSubCommand();

            subCommand
                .BeginSubCommand("instant")
                .WithAlias("i")
                .WithArgs(new WordArgParser("instant", false))
                .HandleWith(OnChangeInstantComplete)
                .EndSubCommand();
        }

        protected override EasySmithingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
        {
            return EasySmithingPacket.Create(
                enabledForPlayer, 
                Settings.CostPerClick, 
                Settings.VoxelsPerClick, 
                Settings.InstantComplete);
        }

        protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
        {
            var sb = new StringBuilder();
            sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
            sb.Append(LangEx.FeatureString("EasySmithing", "CostPerClick", SubCommandName, Settings.CostPerClick));
            sb.Append(LangEx.FeatureString("Knapster", "VoxelsPerClick", SubCommandName, Settings.VoxelsPerClick));
            sb.AppendLine(LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete));
            return TextCommandResult.Success(sb.ToString());
        }

        private TextCommandResult OnChangeCostPerClick(TextCommandCallingArgs args)
        {
            Settings.CostPerClick = GameMath.Clamp(args.RawArgs.PopInt().GetValueOrDefault(1), 1, 10);
            var message = LangEx.FeatureString("EasySmithing", "CostPerClick", SubCommandName, Settings.CostPerClick);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
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
            var input = args.Parsers[0].GetValue().ToString() ?? "d";
            var mode = input! switch
            {
                var d when "disabled".StartsWith(d, true, CultureInfo.InvariantCulture) => false,
                var e when "enabled".StartsWith(e, true, CultureInfo.InvariantCulture) => true,
                _ => false,
            };
            Settings.InstantComplete = mode;
            var message = LangEx.FeatureString("Knapster", "InstantComplete", SubCommandName, Settings.InstantComplete);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(message);
        }
    }
}