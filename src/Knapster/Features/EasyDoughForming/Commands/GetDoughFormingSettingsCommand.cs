namespace Knapster.Features.EasyDoughForming.Commands;

public class GetDoughFormingSettingsCommand(IPlayer player) : CommandBase
{
    public IPlayer Player { get; } = player;
    public bool Enabled { get; internal set; }
    public int VoxelsPerClick { get; internal set; }
    public bool InstantComplete { get; internal set; }

    public class Handler(
        EasyDoughFormingClient client,
        EasyDoughFormingServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetDoughFormingSettingsCommand>
    {
        private readonly EasyDoughFormingClient _client = client;
        private readonly EasyDoughFormingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;

        public override GetDoughFormingSettingsCommand Handle(GetDoughFormingSettingsCommand command)
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

            return base.Handle(command);
        }
    }
}
