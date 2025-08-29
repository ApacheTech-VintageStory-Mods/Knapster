namespace Knapster.Features.EasyPanning.Commands;

public class GetPanningSecondsPerLayerCommand(EntityAgent byEntity) : CommandBase
{
    public EntityAgent ByEntity { get; } = byEntity;
    public float SecondsPerLayer { get; set; } = 4f;

    public class Handler(
        EasyPanningClient client,
        EasyPanningServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetPanningSecondsPerLayerCommand>
    {
        private readonly EasyPanningClient _client = client;
        private readonly EasyPanningServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetPanningSecondsPerLayerCommand Handle(GetPanningSecondsPerLayerCommand command)
        {
            if (command.ByEntity is not EntityPlayer playerEntity) return base.Handle(command);
            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }
            command.SecondsPerLayer = _gantry.ApiEx.Return(
                () => _client.Settings.SecondsPerLayer,
                () => _server.Settings.SecondsPerLayer);
            return base.Handle(command);
        }
    }
}
