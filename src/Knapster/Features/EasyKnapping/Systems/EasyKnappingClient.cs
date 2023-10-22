namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyKnappingClient : ClientModSystem
{
    internal static EasyKnappingPacket Settings = new()
    {
        Enabled = false,
        VoxelsPerClick = 1,
        InstantComplete = false
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyKnappingPacket>()
            .SetMessageHandler<EasyKnappingPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyKnappingPacket packet)
    {
        Settings = packet;
    }
}