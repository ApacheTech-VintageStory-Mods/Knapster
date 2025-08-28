namespace Knapster.Features.EasyTilling.Settings;

/// <summary>
///     Provides access to the EasyTilling settings.
/// </summary>
public class EasyTilling(ICoreGantryAPI api) : SidedService<EasyTillingClient, EasyTillingServer>(api);
