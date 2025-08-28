namespace Knapster.Features.EasySmithing.Commands;

public class GetSmithingSettingsCommand(IPlayer player) : CommandBase
{
    public IPlayer Player { get; } = player;
    public bool Enabled { get; internal set; }
    public int VoxelsPerClick { get; internal set; }
    public bool InstantComplete { get; internal set; }
    public int CostPerClick { get; internal set; }
    public class Handler(
        EasySmithingClient client,
        EasySmithingServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetSmithingSettingsCommand>
    {
        private readonly EasySmithingClient _client = client;
        private readonly EasySmithingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetSmithingSettingsCommand Handle(GetSmithingSettingsCommand command)
        {
            if (command.Player is not EntityPlayer playerEntity) return base.Handle(command);
            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }
            command.Enabled = _gantry.ApiEx.OneOf(
                _client.Settings.Enabled,
                true);
            command.VoxelsPerClick = _gantry.ApiEx.OneOf(
                _client.Settings.VoxelsPerClick,
                _server.Settings.VoxelsPerClick);
            command.InstantComplete = _gantry.ApiEx.OneOf(
                _client.Settings.InstantComplete,
                _server.Settings.InstantComplete);
            command.CostPerClick = _gantry.ApiEx.OneOf(
                _client.Settings.CostPerClick,
                _server.Settings.CostPerClick);
            return base.Handle(command);
        }
    }
}
