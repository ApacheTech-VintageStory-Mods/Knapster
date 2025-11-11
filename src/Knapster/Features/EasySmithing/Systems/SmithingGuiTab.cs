namespace Knapster.Features.EasySmithing.Systems;

public class SmithingGuiTab(ICoreGantryAPI core, EasySmithingServerSettings settings)
    : EasyXGuiTab<EasySmithingServerSettings>(core, "Smithing", settings)
{
    protected override void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // VoxelsPerClick
        composer
            .AddStaticText(F("lblVoxelsPerClick.Text"), LabelFont, EnumTextOrientation.Right, left, "lblVoxelsPerClick")
            .AddAutoSizeHoverText(F("lblVoxelsPerClick.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSliderNew(OnVoxelsPerClickChanged, right, "intVoxelsPerClick");

        // CostPerClick
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(F("lblCostPerClick.Text"), LabelFont, EnumTextOrientation.Right, left, "lblCostPerClick")
            .AddAutoSizeHoverText(F("lblCostPerClick.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddSliderNew(OnCostPerClickChanged, right, "intCostPerClick");

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
        var intVoxelsPerClick = composer.GetSliderNew("intVoxelsPerClick");
        intVoxelsPerClick.SetValues(Settings.VoxelsPerClick, 1, 8, 1, $" {P("intVoxelsPerClick.Unit", Settings.VoxelsPerClick)}");

        var intCostPerClick = composer.GetSliderNew("intCostPerClick");
        intCostPerClick.SetValues(Settings.CostPerClick, 1, 10, 1, $" {F("intCostPerClick.Unit")}");

        composer.GetSwitch("btnInstantComplete").SetValue(Settings.InstantComplete);
    }

    private void OnInstantCompleteChanged(bool instantComplete)
    {
        Settings.InstantComplete = instantComplete;
        RefreshValues(Composer);
    }

    private bool OnCostPerClickChanged(int costPerClick)
    {
        Settings.CostPerClick = costPerClick;
        RefreshValues(Composer);
        return true;
    }

    private bool OnVoxelsPerClickChanged(int voxelsPerClick)
    {
        Settings.VoxelsPerClick = voxelsPerClick;
        RefreshValues(Composer);
        return true;
    }
}