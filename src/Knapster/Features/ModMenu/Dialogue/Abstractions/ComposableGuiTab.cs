using Gantry.GameContent.GUI.Abstractions;

namespace Knapster.Features.ModMenu.Dialogue.Abstractions;

public abstract class ComposableGuiTab : GuiTab, IGuiComposablePart
{
    public abstract ElementBounds Bounds { get; set; }
    public abstract GuiComposer ComposePart(GenericDialogue parent, GuiComposer composer);
    public abstract void RefreshValues(GuiComposer composer);
}
