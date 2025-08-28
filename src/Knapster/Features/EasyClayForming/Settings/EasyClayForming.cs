namespace Knapster.Features.EasyClayForming.Settings;

/// <summary>
///     Provides access to the EasyClayForming settings.
/// </summary>
public class EasyClayForming(ICoreGantryAPI api) : SidedService<EasyClayFormingClient, EasyClayFormingServer>(api);