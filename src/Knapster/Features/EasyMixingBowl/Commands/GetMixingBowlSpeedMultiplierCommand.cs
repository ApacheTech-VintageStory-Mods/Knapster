namespace Knapster.Features.EasyMixingBowl.Commands;

public class GetMixingBowlSpeedMultiplierCommand(List<string> players) : GantryCommandBase
{
    public List<string> Players { get; } = players;
    public float SpeedMultiplier { get; set; } = 1f;

    internal class Handler(
        EasyMixingBowlClient client,
        EasyMixingBowlServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<GetMixingBowlSpeedMultiplierCommand>
    {
        private readonly EasyMixingBowlClient _client = client;
        private readonly EasyMixingBowlServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override async Task HandleAsync(GetMixingBowlSpeedMultiplierCommand command, CancellationToken cancellationToken)
        {
            if (command.Players.Count == 0 && !_gantry.ApiEx.Return(
                () => _client.Settings.IncludeAutomated,
                () => _server.Settings.IncludeAutomated))
                return;

            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledForAll(command.Players)))
                return;

            command.SpeedMultiplier = _gantry.ApiEx.Return(
                () => _client.Settings.SpeedMultiplier,
                () => _server.Settings.SpeedMultiplier);
        }
    }
}
