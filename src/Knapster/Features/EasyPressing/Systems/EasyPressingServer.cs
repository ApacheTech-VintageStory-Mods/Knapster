namespace Knapster.Features.EasyPressing.Systems;

public sealed class EasyPressingServer : EasyXServerSystemBase<EasyPressingServer, EasyPressingServerSettings, EasyPressingClientSettings>
{
    protected override string SubCommandName => "FruitPress";
}