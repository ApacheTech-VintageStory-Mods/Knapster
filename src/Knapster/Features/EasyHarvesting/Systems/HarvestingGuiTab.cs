namespace Knapster.Features.EasyHarvesting.Systems;

public class HarvestingGuiTab(ICoreGantryAPI core, EasyHarvestingServerSettings settings)
    : EasyXGuiTab<EasyHarvestingServerSettings>(core, "Harvesting", settings)
{
    protected override void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // SpeedMultiplier
        composer
            .AddStaticText(F("lblSpeedMultiplier.Text"), LabelFont, EnumTextOrientation.Right, left, "lblSpeedMultiplier")
            .AddAutoSizeHoverText(F("lblSpeedMultiplier.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSlider<float>(OnSpeedMultiplierChanged, right, "fltSpeedMultiplier");
    }

    public override void RefreshValues(GuiComposer composer)
    {
        base.RefreshValues(composer);
        var slider = composer.GetSlider<float>("fltSpeedMultiplier");
        slider.SetValues(Settings.SpeedMultiplier, 0.1f, 2f, 0.05f, 2, "x");
    }

    private bool OnSpeedMultiplierChanged(float speedMultiplier)
    {
        Settings.SpeedMultiplier = speedMultiplier;
        RefreshValues(Composer);
        return true;
    }
}
