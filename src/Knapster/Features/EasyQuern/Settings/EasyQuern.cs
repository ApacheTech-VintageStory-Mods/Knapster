namespace Knapster.Features.EasyQuern.Settings;

/// <summary>
///     Provides access to the EasyQuern settings.
/// </summary>
public class EasyQuern(ICoreGantryAPI api) : SidedService<EasyQuernClient, EasyQuernServer>(api);
