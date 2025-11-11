namespace Knapster.Features.EasyTilling.Systems;

public sealed class EasyTillingServer : EasyXServerSystemBase<EasyTillingServer, EasyTillingServerSettings, EasyTillingClientSettings>
{
    protected override string SubCommandName => "Tilling";
}