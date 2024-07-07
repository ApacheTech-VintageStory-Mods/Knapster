using Gantry.Core.Hosting;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyMixingBowlClient : ClientModSystem
{
    internal static EasyMixingBowlPacket Settings = new()
    {
        Enabled = false,
        SpeedMultiplier = 1f,
        IncludeAutomated = false
    };

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        try
        {
            var shouldLoad = base.ShouldLoad(forSide) && ModEx.IsModEnabled("aculinaryartillery");
            return shouldLoad;
        }
        catch
        {
            return false;
        }
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        IOC.Services.Resolve<IClientNetworkService>()
            .DefaultClientChannel
            .RegisterMessageType<EasyMixingBowlPacket>()
            .SetMessageHandler<EasyMixingBowlPacket>(SyncSettingsWithServer);
    }

    private static void SyncSettingsWithServer(EasyMixingBowlPacket packet)
    {
        Settings = packet;
    }
}