namespace Knapster.Features.EasyPanning.Commands;

public class GetPanningDropsPerLayerCommand(EntityAgent byEntity) : CommandBase
{
    public EntityAgent ByEntity { get; } = byEntity;
    public int DropsPerLayer { get; set; } = 1;

    public class Handler(
        EasyPanningClient client,
        EasyPanningServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetPanningDropsPerLayerCommand>
    {
        private readonly EasyPanningClient _client = client;
        private readonly EasyPanningServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetPanningDropsPerLayerCommand Handle(GetPanningDropsPerLayerCommand command)
        {
            if (command.ByEntity is not EntityPlayer playerEntity) return base.Handle(command);
            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }
            command.DropsPerLayer = _gantry.ApiEx.OneOf(
                _client.Settings.DropsPerLayer,
                _server.Settings.DropsPerLayer);
            return base.Handle(command);
        }
    }
}
