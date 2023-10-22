namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyClayFormingClient : ClientModSystem
{
    internal static EasyClayFormingPacket Settings = new()
    {
        Enabled = false,
        VoxelsPerClick = 1,
        InstantComplete = false
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyClayFormingPacket>()
            .SetMessageHandler<EasyClayFormingPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyClayFormingPacket packet)
    {
        Settings = packet;
    }
}