using Gantry.Core.Hosting;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasySmithingClient : ClientModSystem
{
    internal static EasySmithingPacket Settings = new()
    {
        Enabled = false,
        VoxelsPerClick = 1,
        CostPerClick = 5,
        InstantComplete = false
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasySmithingPacket>()
            .SetMessageHandler<EasySmithingPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasySmithingPacket packet)
    {
        Settings = packet;
    }
}