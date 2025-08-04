using Knapster.Features.EasyPressing.Settings;

namespace Knapster.Features.EasyPressing.Systems;

public sealed class EasyPressingServer : EasyXServerSystemBase<EasyPressingServer, EasyPressingServerSettings, EasyPressingClientSettings, EasyPressingSettings>
{
    protected override string SubCommandName => "FruitPress";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand.WithDescription(G.Lang.FeatureString("EasyPressing", "Description"));
    }
}