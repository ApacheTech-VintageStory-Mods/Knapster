namespace Knapster.Features.EasyQuern.Commands;

public class StickyQuernCommand(EnumItemUseCancelReason cancelReason) : CommandBase
{
    public EnumItemUseCancelReason CancelReason { get; } = cancelReason;

    public class Handler(
        EasyQuernClient client,
        EasyQuernServer server,
        ICoreGantryAPI gantry) : RequestHandler<StickyQuernCommand>
    {
        private readonly EasyQuernClient _client = client;
        private readonly EasyQuernServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;
        public override StickyQuernCommand Handle(StickyQuernCommand command)
        {
            var stickyMouseButton = _gantry.ApiEx.Return(
                    () => _client.Settings.StickyMouseButton,
                    () => _server.Settings.StickyMouseButton);

            command.Success = !(stickyMouseButton && command.CancelReason == EnumItemUseCancelReason.ReleasedMouse);
            return base.Handle(command); ;
        }
    }
}
