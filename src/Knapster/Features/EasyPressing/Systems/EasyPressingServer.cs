using ApacheTech.VintageMods.Knapster.Features.EasyPressing.Settings;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Systems;

[UsedImplicitly]
public sealed class EasyPressingServer : EasyXServerSystemBase<EasyPressingServerSettings, EasyPressingClientSettings, EasyPressingSettings>
{
    protected override string SubCommandName => "FruitPress";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand.WithDescription(LangEx.FeatureString("EasyPressing", "Description"));
    }
}