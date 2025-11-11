namespace Knapster.Features.EasyPanning.Systems;

public sealed class EasyPanningServer : EasyXServerSystemBase<EasyPanningServer, EasyPanningServerSettings, EasyPanningClientSettings>
{
    protected override string SubCommandName => "Panning";
}