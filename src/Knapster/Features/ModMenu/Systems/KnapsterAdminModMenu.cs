using Gantry.Core.Network.Packets;
using Gantry.GameContent.ChatCommands;
using Knapster.Features.ModMenu.Dialogue;
using Knapster.Features.ModMenu.Dialogue.Abstractions;

namespace Knapster.Features.ModMenu.Systems;

[Universal]
public class KnapsterAdminModMenu : UniversalModSystem<KnapsterAdminModMenu>
{
    private IServerNetworkChannel? _serverChannel;
    private IClientNetworkChannel? _clientChannel;

    [ClientSide]
    public List<Func<ComposableGuiTab>> TabFactories { get; } = [];

    public override double ExecuteOrder() => -1;


    public override void StartServerSide(ICoreServerAPI api)
    {
        var command = api.ChatCommands.Get("knapster");

        _serverChannel = api.Network
            .GetOrRegisterDefaultChannel(Core)
            .RegisterPacket<SignalPacket>(Core);

        command.BeginSubCommand("gui")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(G.T("ModMenu", "Description"))
            .HandleWith(p => _serverChannel.SendPacket(SignalPacket.Ping, p.Caller.Player.To<IServerPlayer>()))
            .EndSubCommand();
    }

    public override void StartClientSide(ICoreClientAPI capi)
    {
        G.Log("Starting mod menu service");

        _clientChannel = capi.Network
            .GetOrRegisterDefaultChannel(Core)
            .RegisterPacket<SignalPacket>(Core, ShowSettingsDialogue);
    }

    private void ShowSettingsDialogue(SignalPacket _)
    {
        if (Capi.World.Player.Privileges.Contains(Privilege.controlserver) == false)
        {
            Capi.ShowChatMessage(G.T("ModMenu", "NoPermission"));
            return;
        }
        var tabs = TabFactories.Select(f => f()).ToList();
        var dialogue = new ModMenuDialogue(Core, tabs);
        dialogue.ToggleGui();
    }
}