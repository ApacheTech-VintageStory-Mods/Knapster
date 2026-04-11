
namespace Knapster.Features.EasySmithing.Commands;

public class GetSmithingSettingsCommand(IPlayer player) : GantryCommandBase
{
    public IPlayer Player { get; } = player;
    public bool Enabled { get; internal set; }
    public int VoxelsPerClick { get; internal set; }
    public bool InstantComplete { get; internal set; }
    public int CostPerClick { get; internal set; }
    public class Handler(
        EasySmithingClient client,
        EasySmithingServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<GetSmithingSettingsCommand>
    {
        private readonly EasySmithingClient _client = client;
        private readonly EasySmithingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override async Task HandleAsync(GetSmithingSettingsCommand command, CancellationToken cancellationToken)
        {

            if (command.Player.Entity is not EntityPlayer playerEntity) return;

            command.Enabled = _gantry.ApiEx.Return(
                () => _client.Settings.Enabled,
                () => _server.IsEnabledFor(playerEntity.Player));

            if (!command.Enabled)
                return;

            command.VoxelsPerClick = _gantry.ApiEx.Return(
                () => _client.Settings.VoxelsPerClick,
                () => _server.Settings.VoxelsPerClick);

            command.InstantComplete = _gantry.ApiEx.Return(
                () => _client.Settings.InstantComplete,
                () => _server.Settings.InstantComplete);

            command.CostPerClick = _gantry.ApiEx.Return(
                () => _client.Settings.CostPerClick,
                () => _server.Settings.CostPerClick);

            return;
        }
    }
}
