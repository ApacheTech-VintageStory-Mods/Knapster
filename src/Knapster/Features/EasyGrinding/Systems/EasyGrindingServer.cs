namespace Knapster.Features.EasyGrinding.Systems;

/// <summary>
///     Server-side system for the EasyGrinding feature, providing chat commands and settings management for quern automation and speed.
/// </summary>
public class EasyGrindingServer : EasyXServerSystemBase<EasyGrindingServer, EasyGrindingServerSettings, EasyGrindingClientSettings>
{
    protected override string SubCommandName => "Grinding";
}