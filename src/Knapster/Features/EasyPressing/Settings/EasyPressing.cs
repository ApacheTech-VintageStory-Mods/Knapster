namespace Knapster.Features.EasyPressing.Settings;

/// <summary>
///     Provides access to the EasyPressing settings.
/// </summary>
public class EasyPressing(ICoreGantryAPI api) : SidedService<EasyPressingClient, EasyPressingServer>(api);
