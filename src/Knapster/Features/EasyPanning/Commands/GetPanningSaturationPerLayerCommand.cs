namespace Knapster.Features.EasyPanning.Commands;

public class GetPanningSaturationPerLayerCommand(EntityAgent byEntity) : CommandBase
{
    public EntityAgent ByEntity { get; } = byEntity;
    public float SaturationPerLayer { get; set; } = 3f;

    public class Handler(
        EasyPanningClient client,
        EasyPanningServer server,
        ICoreGantryAPI gantry) : RequestHandler<GetPanningSaturationPerLayerCommand>
    {
        private readonly EasyPanningClient _client = client;
        private readonly EasyPanningServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override GetPanningSaturationPerLayerCommand Handle(GetPanningSaturationPerLayerCommand command)
        {
            if (command.ByEntity is not EntityPlayer playerEntity) return base.Handle(command);
            if (!_gantry.ApiEx.Return(
                    () => _client.Settings.Enabled,
                    () => _server.IsEnabledFor(playerEntity.Player)))
            {
                return base.Handle(command);
            }
            command.SaturationPerLayer = _gantry.ApiEx.OneOf(
                _client.Settings.SaturationPerLayer,
                _server.Settings.SaturationPerLayer);
            return base.Handle(command);
        }
    }
}
