namespace Knapster.Features.EasyQuern.Commands;

public class GetQuernSpeedMultiplierCommand(List<string> players) : CommandBase
{
    public List<string> Players { get; set; } = players;
    public float SpeedMultiplier { get; set; } = 1f;
    internal class Handler(
        EasyQuernClient client,
        EasyQuernServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetQuernSpeedMultiplierCommand>
    {
        private readonly EasyQuernClient _client = client;
        private readonly EasyQuernServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetQuernSpeedMultiplierCommand Handle(GetQuernSpeedMultiplierCommand command)
        {
            command.SpeedMultiplier = command.Players.Count == 0 && !IncludeAutomated
                ? 1f : !EnabledForAll(command.Players) 
                ? 1f : SpeedMultiplier;

            return base.Handle(command);
        }

        private float SpeedMultiplier => _gantry.ApiEx.Return(
            () => _client.Settings.SpeedMultiplier,
            () => _server.Settings.SpeedMultiplier);

        private bool IncludeAutomated => _gantry.ApiEx.Return(
            () => _client.Settings.IncludeAutomated,
            () => _server.Settings.IncludeAutomated);

        private bool EnabledForAll(IEnumerable<string> players) 
            => _gantry.ApiEx.Return(
                () => _client.Settings.Enabled,
                () => _server.IsEnabledForAll(players));
    }
}   
