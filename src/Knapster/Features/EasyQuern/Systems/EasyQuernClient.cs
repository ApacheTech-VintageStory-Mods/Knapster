namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyQuernClient : ClientModSystem
{
    internal static EasyQuernPacket Settings = new()
    {
        Enabled = false,
        SpeedMultiplier = 1f,
        IncludeAutomated = false
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyQuernPacket>()
            .SetMessageHandler<EasyQuernPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyQuernPacket packet)
    {
        Settings = packet;
    }
}