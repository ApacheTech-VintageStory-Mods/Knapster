namespace Knapster.Features.EasyKnapping.Settings;

/// <summary>
///     Provides access to the EasyKnapping settings.
/// </summary>
public class EasyKnapping(ICoreGantryAPI api) : SidedService<EasyKnappingClient, EasyKnappingServer>(api);
