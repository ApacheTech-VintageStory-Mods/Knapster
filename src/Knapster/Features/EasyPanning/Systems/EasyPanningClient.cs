namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Systems
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class EasyPanningClient : ClientModSystem
    {
        internal static EasyPanningPacket Settings = new()
        {
            Enabled = false,
            SpeedMultiplier = 1f,
            SaturationMultiplier = 1f
        };

        public override void StartClientSide(ICoreClientAPI api)
        {
            IOC.Services.Resolve<IClientNetworkService>()
                .DefaultClientChannel
                .RegisterMessageType<EasyPanningPacket>()
                .SetMessageHandler<EasyPanningPacket>(SyncSettingsWithServer);
        }

        private static void SyncSettingsWithServer(EasyPanningPacket packet)
        {
            Settings = packet;
        }
    }
}