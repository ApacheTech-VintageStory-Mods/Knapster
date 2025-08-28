namespace Knapster.Features.EasyHarvesting.Commands;

public class GetHarvestingSpeedMultiplierCommand(EntityAgent byEntity) : CommandBase
{
    public EntityAgent ByEntity { get; } = byEntity;
    public float SpeedMultiplier { get; set; } = 1f;
    public class Handler(
        EasyHarvestingClient client,
        EasyHarvestingServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetHarvestingSpeedMultiplierCommand>
    {
        private readonly EasyHarvestingClient _client = client;
        private readonly EasyHarvestingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetHarvestingSpeedMultiplierCommand Handle(GetHarvestingSpeedMultiplierCommand command)
        {
            if (command.ByEntity is not EntityPlayer playerEntity) return base.Handle(command);
            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }
            command.SpeedMultiplier = _gantry.ApiEx.OneOf(
                _client.Settings.SpeedMultiplier,
                _server.Settings.SpeedMultiplier);
            return base.Handle(command);
        }
    }
}