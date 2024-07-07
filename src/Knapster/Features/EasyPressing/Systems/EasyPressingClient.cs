using Gantry.Core.Hosting;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyPressingClient : ClientModSystem
{
    internal static EasyPressingPacket Settings = new()
    {
        Enabled = false
    };

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyPressingPacket>()
            .SetMessageHandler<EasyPressingPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyPressingPacket packet)
    {
        Settings = packet;
    }
}