namespace Knapster.Features.EasyMixingBowl.Commands;

public class StickyMixingBowlCommand(EnumItemUseCancelReason cancelReason) : CommandBase
{
    public EnumItemUseCancelReason CancelReason { get; } = cancelReason;

    public class Handler(
        EasyMixingBowlClient client,
        EasyMixingBowlServer server,
        ICoreGantryAPI gantry) : RequestHandler<StickyMixingBowlCommand>
    {
        private readonly EasyMixingBowlClient _client = client;
        private readonly EasyMixingBowlServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;

        [RequiresMod("aculinaryartillery")]
        public override StickyMixingBowlCommand Handle(StickyMixingBowlCommand command)
        {
            var stickyMouseButton = _gantry.ApiEx.Return(
                () => _client.Settings.StickyMouseButton,
                () => _server.Settings.StickyMouseButton);

            command.Success = !(stickyMouseButton && command.CancelReason == EnumItemUseCancelReason.ReleasedMouse);
            return base.Handle(command);
        }
    }
}