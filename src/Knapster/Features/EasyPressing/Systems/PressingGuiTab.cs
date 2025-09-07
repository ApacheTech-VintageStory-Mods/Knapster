using Knapster.Features.ModMenu.Dialogue.Abstractions;

namespace Knapster.Features.EasyPressing.Systems;

public class PressingGuiTab(EasyPressingServerSettings settings)
    : EasyXGuiTab<EasyPressingServerSettings>("EasyPressing", settings);
