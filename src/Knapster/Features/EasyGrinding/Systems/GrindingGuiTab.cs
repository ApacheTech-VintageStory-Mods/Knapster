namespace Knapster.Features.EasyGrinding.Systems;

public class GrindingGuiTab(EasyGrindingServerSettings settings)
    : EasyXGuiTab<EasyGrindingServerSettings>("EasyGrinding", settings)
{
    protected override void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // SpeedMultiplier
        composer
            .AddStaticText(F("lblSpeedMultiplier.Text"), LabelFont, EnumTextOrientation.Right, left, "lblSpeedMultiplier")
            .AddAutoSizeHoverText(F("lblSpeedMultiplier.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSlider<float>(OnSpeedMultiplierChanged, right, "fltSpeedMultiplier");

        // IncludeAutomated
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lbIncludeAutomated.Text"), LabelFont, EnumTextOrientation.Right, left, "lblIncludeAutomated")
            .AddAutoSizeHoverText(F("lblIncludeAutomated.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSwitch(OnIncludeAutomatedChanged, right, "btnIncludeAutomated");

        // StickyMouseButton
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lbStickyMouseButton.Text"), LabelFont, EnumTextOrientation.Right, left, "lblStickyMouseButton")
            .AddAutoSizeHoverText(F("lblStickyMouseButton.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSwitch(OnStickyMouseButtonChanged, right, "btnStickyMouseButton");
    }

    public override void RefreshValues(GuiComposer composer)
    {
        base.RefreshValues(composer);
        var slider = composer.GetSlider<float>("fltSpeedMultiplier");
        slider.SetValues(Settings.SpeedMultiplier, 0.1f, 10f, 0.05f, 2, "x");
        composer.GetSwitch("btnIncludeAutomated").SetValue(Settings.IncludeAutomated);
        composer.GetSwitch("btnStickyMouseButton").SetValue(Settings.StickyMouseButton);
    }

    private void OnStickyMouseButtonChanged(bool stickyMouseButton)
    {
        Settings.StickyMouseButton = stickyMouseButton;
        RefreshValues(Composer);
    }

    private void OnIncludeAutomatedChanged(bool includeAutomated)
    {
        Settings.IncludeAutomated = includeAutomated;
        RefreshValues(Composer);
    }

    private bool OnSpeedMultiplierChanged(float speedMultiplier)
    {
        Settings.SpeedMultiplier = speedMultiplier;
        RefreshValues(Composer);
        return true;
    }
}
