using Gantry.Services.Mediator.Filters;

namespace Knapster.Features.EasyMixingBowl.Commands;

public class StickyMixingBowlCommand(EnumItemUseCancelReason cancelReason) : GantryCommandBase
{
    public EnumItemUseCancelReason CancelReason { get; } = cancelReason;

    public class Handler(
        EasyMixingBowlClient client,
        EasyMixingBowlServer server,
        ICoreGantryAPI gantry) : CommandHandlerBase<StickyMixingBowlCommand>
    {
        private readonly EasyMixingBowlClient _client = client;
        private readonly EasyMixingBowlServer _server = server;
        private readonly ICoreGantryAPI _gantry = gantry;

        [RequiresMod("aculinaryartillery")]
        public override async Task HandleAsync(StickyMixingBowlCommand command, CancellationToken cancellationToken)
        {
            var stickyMouseButton = _gantry.ApiEx.Return(
                () => _client.Settings.StickyMouseButton,
                () => _server.Settings.StickyMouseButton);

            command.Success = !(stickyMouseButton && command.CancelReason == EnumItemUseCancelReason.ReleasedMouse);
        }
    }
}