using Knapster.Features.ModMenu.Systems;

namespace Knapster.Features.ModMenu.Dialogue;

internal class GeneralSettingsGuiTab : ComposableGuiTab
{
    private readonly IClientNetworkChannel? _clientChannel;
    private ModFileScope _scope;

    public GeneralSettingsGuiTab(ICoreGantryAPI gantry, ModFileScope scope)
    {
        Name = T("TabName");
        _clientChannel = gantry.Capi.Network.GetDefaultChannel(gantry);
        _scope = scope;
    }

    public override ElementBounds Bounds { get; set; } = ElementBounds.Fixed(0, 25, 600, 30);

    public override GuiComposer ComposePart(GenericDialogue parent, GuiComposer composer)
    {
        // Title
        composer.AddStaticText(T("lblTitle.Text"), CairoFont.WhiteSmallishText().WithWeight(Cairo.FontWeight.Bold), Bounds, "lblTitle");
        
        // Description
        SetRowBounds(Bounds, Bounds, out var left, out var right);
        composer.AddStaticText(T("lblDescription.Text"), LabelFont, EnumTextOrientation.Justify, Bounds.BelowCopy(0, 10), "lblDescription");

        // Scope
        string[] scopes = ["World", "Global"];
        var scopeNames = scopes.Select(s => T($"cbxScope.Option.{s}")).ToArray();
        SetRowBounds(left, right, out left, out right);
        composer
            .AddStaticText(T("lblScope.Text"), LabelFont, EnumTextOrientation.Right, left, "lblScope")
            .AddAutoSizeHoverText(T("lblScope.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddDropDown(scopes, scopeNames, 0, OnScopeChanged, right, "cbxScope");

        // Save Button
        var controlRowBoundsRightFixed = ElementBounds.FixedSize(150, 30).WithFixedOffset(0, 25f).WithAlignment(EnumDialogArea.RightTop);
        composer
            .AddSmallButton(T("btnSaveChanges.Text"), OnSaveButtonPressed, controlRowBoundsRightFixed, EnumButtonStyle.Small, "btnSaveChanges");

        return composer;
    }

    public override void RefreshValues(GuiComposer composer)
    {
        if (_scope == ModFileScope.Gantry) _scope = ModFileScope.World;
        composer.GetDropDown("cbxScope")?.SetSelectedIndex(_scope == ModFileScope.World ? 0 : 1);        
    }

    private void OnScopeChanged(string code, bool selected)
    {
        _scope = Enum.Parse<ModFileScope>(code);
    }

    private bool OnSaveButtonPressed()
    {
        _clientChannel?.SendPacket<ModMenuSettingsPacket>(new() { Scope = _scope });
        return true;
    }

    private static string T(string code, params object[] args)
        => G.T("ModMenu.GeneralSettings", code, args);
}