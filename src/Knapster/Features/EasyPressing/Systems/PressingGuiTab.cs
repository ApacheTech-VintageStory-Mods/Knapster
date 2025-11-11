namespace Knapster.Features.EasyPressing.Systems;

public class PressingGuiTab(ICoreGantryAPI core, EasyPressingServerSettings settings)
    : EasyXGuiTab<EasyPressingServerSettings>(core, "Pressing", settings);
