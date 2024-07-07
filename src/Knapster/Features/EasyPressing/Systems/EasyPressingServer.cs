using ApacheTech.VintageMods.Knapster.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Systems;

[UsedImplicitly]
public sealed class EasyPressingServer : FeatureServerSystemBase<EasyPressingSettings, EasyPressingPacket>
{
    protected override string SubCommandName => "FruitPress";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand.WithDescription(LangEx.FeatureString("EasyPressing", "Description"));
    }

    protected override EasyPressingPacket GeneratePacketPerPlayer(IPlayer player, bool enabledForPlayer)
    {
        return EasyPressingPacket.Create(enabledForPlayer);
    }

    protected override TextCommandResult DisplayInfo(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(LangEx.FeatureString("Knapster", "Mode", SubCommandName, Settings.Mode));
        return TextCommandResult.Success(sb.ToString());
    }
}