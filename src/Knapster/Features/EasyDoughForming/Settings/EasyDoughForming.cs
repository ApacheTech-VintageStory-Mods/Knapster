namespace Knapster.Features.EasyDoughForming.Settings;

/// <summary>
///     Provides access to the EasyDoughForming settings.
/// </summary>
public class EasyDoughForming(ICoreGantryAPI api) : SidedService<EasyDoughFormingClient, EasyDoughFormingServer>(api);
