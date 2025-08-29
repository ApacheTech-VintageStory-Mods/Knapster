namespace Knapster.Features.EasyTilling.Commands;

internal class GetTillingSpeedMultiplierCommand(EntityAgent agent) : CommandBase
{
    public EntityAgent Agent { get; set; } = agent;

    public float TillingSpeedMultiplier { get; set; } = 1f;

    internal class Handler(
        EasyTillingClient client,
        EasyTillingServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetTillingSpeedMultiplierCommand>
    {
        private readonly EasyTillingClient _client = client;
        private readonly EasyTillingServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;

        public override GetTillingSpeedMultiplierCommand Handle(GetTillingSpeedMultiplierCommand command)
        {
            if (command.Agent is not EntityPlayer playerEntity) return base.Handle(command);

            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }

            command.TillingSpeedMultiplier = _gantry.ApiEx.Return(
                () => _client.Settings.SpeedMultiplier,
                () => _server.Settings.SpeedMultiplier);

            return base.Handle(command);
        }
    }
}