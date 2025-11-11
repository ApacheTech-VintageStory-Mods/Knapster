namespace Knapster.Features.EasyDoughForming.Systems;

public class DoughFormingGuiTab(ICoreGantryAPI core, EasyDoughFormingServerSettings settings)
    : EasyXGuiTab<EasyDoughFormingServerSettings>(core, "DoughForming", settings)
{
    protected override void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // VoxelsPerClick
        composer
            .AddStaticText(F("lblVoxelsPerClick.Text"), LabelFont, EnumTextOrientation.Right, left, "lblVoxelsPerClick")
            .AddAutoSizeHoverText(F("lblVoxelsPerClick.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSliderNew(OnVoxelsPerClickChanged, right, "intVoxelsPerClick");

        // InstantComplete
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lblInstantComplete.Text"), LabelFont, EnumTextOrientation.Right, left, "lblInstantComplete")
            .AddAutoSizeHoverText(F("lblInstantComplete.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSwitch(OnInstantCompleteChanged, right, "btnInstantComplete");
    }

    public override void RefreshValues(GuiComposer composer)
    {
        base.RefreshValues(composer);
        var slider = composer.GetSliderNew("intVoxelsPerClick");
        slider.SetValues(Settings.VoxelsPerClick, 1, 8, 1, $" {P("intVoxelsPerClick.Unit", Settings.VoxelsPerClick)}");
        composer.GetSwitch("btnInstantComplete").SetValue(Settings.InstantComplete);
    }

    private void OnInstantCompleteChanged(bool instantComplete)
    {
        Settings.InstantComplete = instantComplete;
        RefreshValues(Composer);
    }

    private bool OnVoxelsPerClickChanged(int voxelsPerClick)
    {
        Settings.VoxelsPerClick = voxelsPerClick;
        RefreshValues(Composer);
        return true;
    }
}
