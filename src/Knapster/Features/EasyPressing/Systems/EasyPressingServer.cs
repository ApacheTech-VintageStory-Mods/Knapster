namespace Knapster.Features.EasyPressing.Systems;

public sealed class EasyPressingServer : EasyXServerSystemBase<EasyPressingServer, EasyPressingServerSettings, EasyPressingClientSettings>
{
    protected override string SubCommandName => "FruitPress";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand.WithDescription(G.T("EasyPressing", "Description"));
    }
}