namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyHarvestingClient : ClientModSystem
{
    internal static EasyHarvestingPacket Settings = new()
    {
        Enabled = false,
        SpeedMultiplier = 1f
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyHarvestingPacket>()
            .SetMessageHandler<EasyHarvestingPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyHarvestingPacket packet)
    {
        Settings = packet;
    }
}