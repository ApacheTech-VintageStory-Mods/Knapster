namespace Knapster.Features.EasyDoughForming.Commands;

public class GetDoughFormingSettingsCommand(IPlayer player) : GantryCommandBase
{
    public IPlayer Player { get; } = player;
    public bool Enabled { get; internal set; }
    public int VoxelsPerClick { get; internal set; } = 1;
    public bool InstantComplete { get; internal set; }

    public class Handler(
        EasyDoughFormingClient client,
        EasyDoughFormingServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<GetDoughFormingSettingsCommand>
    {
        private readonly EasyDoughFormingClient _client = client;
        private readonly EasyDoughFormingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;

        public override async Task HandleAsync(GetDoughFormingSettingsCommand command, CancellationToken cancellationToken)
        {
            if (command.Player.Entity is not EntityPlayer playerEntity) return;

            command.Enabled = _gantry.ApiEx.Return(
                () => _client.Settings.Enabled,
                () => _server.IsEnabledFor(playerEntity.Player));

            command.VoxelsPerClick = _gantry.ApiEx.Return(
                () => _client.Settings.VoxelsPerClick,
                () => _server.Settings.VoxelsPerClick);

            command.InstantComplete = _gantry.ApiEx.Return(
                () => _client.Settings.InstantComplete,
                () => _server.Settings.InstantComplete);

            return;
        }
    }
}
