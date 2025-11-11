namespace Knapster.Features.EasyClayForming.Systems;

public sealed class EasyClayFormingServer : EasyXServerSystemBase<EasyClayFormingServer, EasyClayFormingServerSettings, EasyClayFormingClientSettings>
{
    protected override string SubCommandName => "ClayForming";
}