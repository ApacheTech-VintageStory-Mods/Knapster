namespace Knapster.Features.EasySmithing.Settings;

/// <summary>
///     Provides access to the EasySmithing settings.
/// </summary>
public class EasySmithing(ICoreGantryAPI api) : SidedService<EasySmithingClient, EasySmithingServer>(api);