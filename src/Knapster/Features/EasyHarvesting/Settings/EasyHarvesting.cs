namespace Knapster.Features.EasyHarvesting.Settings;

/// <summary>
///     Provides access to the EasyHarvesting settings.
/// </summary>
public class EasyHarvesting(ICoreGantryAPI api) : SidedService<EasyHarvestingClient, EasyHarvestingServer>(api);