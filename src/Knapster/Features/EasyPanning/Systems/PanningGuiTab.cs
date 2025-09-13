namespace Knapster.Features.EasyPanning.Systems;

public class PanningGuiTab(EasyPanningServerSettings settings)
    : EasyXGuiTab<EasyPanningServerSettings>("EasyPanning", settings)
{
    protected override void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // SecondsPerLayer
        composer
            .AddStaticText(F("lblSecondsPerLayer.Text"), LabelFont, EnumTextOrientation.Right, left, "lblSecondsPerLayer")
            .AddAutoSizeHoverText(F("lblSecondsPerLayer.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSlider<float>(OnSecondsPerLayerChanged, right, "fltSecondsPerLayer");

        // DropsPerLayer
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lblDropsPerLayer.Text"), LabelFont, EnumTextOrientation.Right, left, "lblDropsPerLayer")
            .AddAutoSizeHoverText(F("lblDropsPerLayer.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSlider<int>(OnDropsPerLayerChanged, right, "intDropsPerLayer");

        // SaturationPerLayer
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lblSaturationPerLayer.Text"), LabelFont, EnumTextOrientation.Right, left, "lblSaturationPerLayer")
            .AddAutoSizeHoverText(F("lblSaturationPerLayer.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSlider<float>(OnSaturationPerLayerChanged, right, "fltSaturationPerLayer");
    }

    public override void RefreshValues(GuiComposer composer)
    {
        base.RefreshValues(composer);

        var fltSecondsPerLayer = composer.GetSlider<float>("fltSecondsPerLayer");
        fltSecondsPerLayer.SetValues(Settings.SecondsPerLayer, 0.1f, 10f, 0.1f, 1, $" {P("fltSecondsPerLayer.Unit", Settings.SecondsPerLayer)}");

        var intDropsPerLayer = composer.GetSliderNew("intDropsPerLayer");
        intDropsPerLayer.SetValues(Settings.DropsPerLayer, 0, 10, 1, $" {P("intDropsPerLayer.Unit", Settings.DropsPerLayer)}");
        
        composer.GetSlider<float>("fltSaturationPerLayer").SetValues(Settings.SaturationPerLayer, 0.1f, 10f, 0.1f, 1, $" {F("fltSaturationPerLayer.Unit")}");
    }

    private bool OnSecondsPerLayerChanged(float secondsPerLayer)
    {
        Settings.SecondsPerLayer = secondsPerLayer;
        RefreshValues(Composer);
        return true;
    }

    private bool OnDropsPerLayerChanged(int dropsPerLayer)
    {
        Settings.DropsPerLayer = dropsPerLayer;
        RefreshValues(Composer);
        return true;
    }

    private bool OnSaturationPerLayerChanged(float saturationPerLayer)
    {
        Settings.SaturationPerLayer = saturationPerLayer;
        RefreshValues(Composer);
        return true;
    }
}
