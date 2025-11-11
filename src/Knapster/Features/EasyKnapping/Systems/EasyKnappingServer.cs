namespace Knapster.Features.EasyKnapping.Systems;

public sealed class EasyKnappingServer : EasyXServerSystemBase<EasyKnappingServer, EasyKnappingServerSettings, EasyKnappingClientSettings>
{
    protected override string SubCommandName => "Knapping";
}