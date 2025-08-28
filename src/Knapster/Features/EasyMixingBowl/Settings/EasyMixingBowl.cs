namespace Knapster.Features.EasyMixingBowl.Settings;

/// <summary>
///     Provides access to the EasyMixingBowl settings.
/// </summary>
public class EasyMixingBowl(ICoreGantryAPI api) : SidedService<EasyMixingBowlClient, EasyMixingBowlServer>(api);
