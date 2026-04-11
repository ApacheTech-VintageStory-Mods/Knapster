namespace Knapster.Features.EasyHarvesting.Commands;

public class GetHarvestingSpeedMultiplierCommand(EntityAgent byEntity) : GantryCommandBase
{
    public EntityAgent ByEntity { get; } = byEntity;
    public float SpeedMultiplier { get; set; } = 1f;
    public class Handler(
        EasyHarvestingClient client,
        EasyHarvestingServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<GetHarvestingSpeedMultiplierCommand>
    {
        private readonly EasyHarvestingClient _client = client;
        private readonly EasyHarvestingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override async Task HandleAsync(GetHarvestingSpeedMultiplierCommand command, CancellationToken cancellationToken)
        {
            if (command.ByEntity is not EntityPlayer playerEntity) return;
            if (!_gantry.ApiEx.Return(
                () => _client.Settings.Enabled,
                () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return;
            }
            command.SpeedMultiplier = _gantry.ApiEx.Return(
                () => _client.Settings.SpeedMultiplier,
                () => _server.Settings.SpeedMultiplier);
        }
    }
}