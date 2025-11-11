namespace Knapster.Features.EasySmithing.Systems;

public class EasySmithingServer : EasyXServerSystemBase<EasySmithingServer, EasySmithingServerSettings, EasySmithingClientSettings>
{
    protected override string SubCommandName => "Smithing";
}