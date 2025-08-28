namespace Knapster.Features.EasyPanning.Settings;

/// <summary>
///     Provides access to the EasyPanning settings.
/// </summary>
public class EasyPanning(ICoreGantryAPI api) : SidedService<EasyPanningClient, EasyPanningServer>(api);
