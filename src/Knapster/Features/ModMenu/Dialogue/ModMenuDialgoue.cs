using Gantry.GameContent.GUI.Abstractions;
using Gantry.GameContent.GUI.Helpers;
using Knapster.Features.ModMenu.Dialogue.Abstractions;

namespace Knapster.Features.ModMenu.Dialogue;

internal class ModMenuDialogue : GenericDialogue
{
    private ComposableGuiTab _currentTab;
    private readonly List<ComposableGuiTab> _tabs = [];
    private readonly ElementBounds _leftTabBounds;

    private static string T(string code, params object[] args) 
        => G.T("ModMenu", code, args);

    public ModMenuDialogue(ICoreGantryAPI gantry, List<ComposableGuiTab> tabs) : base(gantry)
    {
        Title = T("Title");
        Alignment = EnumDialogArea.CenterMiddle;
        ModalTransparency = 0.6f;
        ShowTitleBar = true;
        Modal = true;

        var font = CairoFont.WhiteDetailText().WithFontSize(17f);
        var maxWidth = 200.0;
        foreach (var tab in tabs.OrderBy(p => p.Name))
        {
            tab.Active = false;
            tab.DataInt = _tabs.Count;
            var width = font.GetTextExtents(tab.Name).Width;
            maxWidth = Math.Max(width + 1.0 + 2.0 * GuiElement.scaled(3.0), maxWidth);
            _tabs.Add(tab);
        }
        _currentTab = _tabs.First().To<ComposableGuiTab>();
        _leftTabBounds = ElementBounds.Fixed(-221f, 20f, maxWidth, _tabs.Count * 29f);
    }

    protected override void ComposeBody(GuiComposer composer)
    {
        composer.AddVerticalTabs([.. _tabs], _leftTabBounds, OnTabClicked, "tabMenuLeft");
        composer.AddComposablePart(this, _currentTab);
    }

    protected override void RefreshValues()
    {
        _currentTab.RefreshValues(SingleComposer);
        var tabs = SingleComposer.GetVerticalTab("tabMenuLeft");
        tabs.SetValue(_currentTab.DataInt, triggerHandler: false);
    }

    private void OnTabClicked(int arg1, GuiTab tab)
    {
        _tabs.ForEach(t => t.Active = false);
        _currentTab = tab.To<ComposableGuiTab>();
        _currentTab.Active = true;
        Recompose();
    }
}