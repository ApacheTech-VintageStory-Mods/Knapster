using Gantry.Services.EasyX;
using Knapster.Features.ModMenu.Dialogue;

namespace Knapster.Features.ModMenu.Systems;

[Universal]
public class KnapsterAdminModMenu : UniversalModSystem<KnapsterAdminModMenu>
{
    [Universal]
    public override double ExecuteOrder() => -1;

    [ServerSide]
    private ConfigurationSettings? _settings;

    [ServerSide]
    private IServerNetworkChannel? _serverChannel;

    [ServerSide]
    public override void StartServerSide(ICoreServerAPI api)
    {
        var command = api.ChatCommands.Get("knapster");
        _settings = G.Settings.Global.Feature<ConfigurationSettings>();

        _serverChannel = api.Network
            .GetOrRegisterDefaultChannel(Core)
            .RegisterPacket<ModMenuSettingsPacket>(Core, SetScope);

        command.BeginSubCommand("gui")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(G.T("ModMenu", "Description"))
            .HandleWith(SendGuiRequest)
            .EndSubCommand();
    }

    [ServerSide]
    private void SetScope(IServerPlayer player, ModMenuSettingsPacket packet)
    {
        if (player.Privileges.Contains(Privilege.controlserver) == false) return;
        if (packet?.Scope is null) return;
        Program.Instance.SetScope(packet.Scope);
        _serverChannel?.SendPacket(new SettingsSavedPacket { FeatureName = $"GeneralSettings" }, player);
    }

    [ServerSide]
    private TextCommandResult SendGuiRequest(TextCommandCallingArgs args)
    {
        var packet = new ModMenuSettingsPacket { Scope = _settings!.Scope };
        _serverChannel?.SendPacket(packet, args.Caller.Player as IServerPlayer);
        return TextCommandResult.Success();
    }

    [ClientSide]
    private IClientNetworkChannel? _clientChannel;

    [ClientSide]
    public List<Func<ComposableGuiTab>> TabFactories { get; } = [];

    [ClientSide]
    public override void StartClientSide(ICoreClientAPI capi)
    {
        G.Log("Starting mod menu service");

        _clientChannel = capi.Network
            .GetOrRegisterDefaultChannel(Core)
            .RegisterPacket<ModMenuSettingsPacket>(Core, ShowSettingsDialogue);
    }

    [ClientSide]
    private void ShowSettingsDialogue(ModMenuSettingsPacket packet)
    {
        if (Capi.World.Player.Privileges.Contains(Privilege.controlserver) == false)
        {
            Capi.ShowChatMessage(G.T("ModMenu", "NoPermission"));
            return;
        }
        var tabs = TabFactories.Select(f => f()).ToList();
        var dialogue = new ModMenuDialogue(Core, tabs, packet.Scope);
        dialogue.ToggleGui();
    }
}