namespace Knapster.Features.ModMenu.Dialogue.Abstractions;

public abstract class ComposableGuiTab : GuiTab, IGuiComposablePart
{
    protected const int HOVER_TEXT_WIDTH = 260;
    public abstract ElementBounds Bounds { get; set; }
    public abstract GuiComposer ComposePart(GenericDialogue parent, GuiComposer composer);
    public abstract void RefreshValues(GuiComposer composer);
    protected CairoFont LabelFont { get; } = CairoFont.WhiteSmallText();
    protected CairoFont HoverTextFont { get; } = CairoFont.WhiteDetailText();


    protected void SetRowBounds(ElementBounds leftUnder, ElementBounds rightUnder, out ElementBounds left, out ElementBounds right)
    {
        var leftPad = leftUnder == Bounds ? 15 : 10;
        left = ElementBounds.FixedSize(200, 30).FixedUnder(leftUnder, leftPad);
        right = ElementBounds.FixedSize(370, 30).FixedUnder(rightUnder, 10).FixedRightOf(left, 10);
    }
}
