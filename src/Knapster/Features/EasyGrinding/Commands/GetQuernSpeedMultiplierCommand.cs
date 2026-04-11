namespace Knapster.Features.EasyGrinding.Commands;

public class GetGrindingSpeedMultiplierCommand(List<string> players) : GantryCommandBase
{
    public List<string> Players { get; set; } = players;
    public float SpeedMultiplier { get; set; } = 1f;
    internal class Handler(
        EasyGrindingClient client,
        EasyGrindingServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<GetGrindingSpeedMultiplierCommand>
    {
        private readonly EasyGrindingClient _client = client;
        private readonly EasyGrindingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override async Task HandleAsync(GetGrindingSpeedMultiplierCommand command, CancellationToken cancellationToken)
        {
            command.SpeedMultiplier = command.Players.Count == 0 && !IncludeAutomated
                ? 1f : !EnabledForAll(command.Players) 
                ? 1f : SpeedMultiplier;

            return;
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
