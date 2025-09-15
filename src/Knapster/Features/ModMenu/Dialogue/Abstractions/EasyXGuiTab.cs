using System.Numerics;
using Vintagestory.API.Util;

namespace Knapster.Features.ModMenu.Dialogue.Abstractions;

public abstract class EasyXGuiTab<TSettings> : ComposableGuiTab
    where TSettings : FeatureSettings<TSettings>, IEasyXServerSettings, new()
{
    private string _selectedWhitelistablePlayerId = string.Empty;
    private string _selectedWhitelistedPlayerId = string.Empty;
    private string _selectedBlacklistablePlayerId = string.Empty;
    private string _selectedBlacklistedPlayerId = string.Empty;

    public override ElementBounds Bounds { get; set; } = ElementBounds.Fixed(0, 25, 500, 30);
    protected string FeatureName { get; }
    protected TSettings Settings { get; }
    protected IClientNetworkChannel? ClientChannel { get; } = null!;
    protected GenericDialogue? Parent { get; set; }
    protected GuiComposer Composer { get; private set; } = null!;

    protected EasyXGuiTab(string featureName, TSettings settings)
    {
        FeatureName = featureName;
        Settings = settings;
        ClientChannel = G.Capi.Network.GetDefaultChannel(G.Core);
        Name = F("TabName");
    }

    protected string F(string code, params object[] args)
        => G.T($"{FeatureName}.ModMenu", code, args);

    protected string L(string code, params object[] args)
        => G.T("ModMenu", code, args);

    protected string P<T>(string code, T amount) where T : struct, INumber<T>
        => G.Lang.Pluralise(G.Lang.Code(FeatureName, $"ModMenu.{code}"), amount);

    public override GuiComposer ComposePart(GenericDialogue parent, GuiComposer composer)
    {
        Parent = parent;
        Composer = composer;

        // Title
        composer.AddStaticText(F("lblTitle.Text"), CairoFont.WhiteSmallishText().WithWeight(Cairo.FontWeight.Bold), Bounds, "lblTitle");

        // Description
        SetRowBounds(Bounds, Bounds, out var left, out var right);
        composer.AddStaticText(F("lblDescription.Text"), LabelFont, EnumTextOrientation.Justify, left.WithFixedWidth(570), "lblDescription");

        // Access Mode
        SetRowBounds(left, right, out left, out right);
        ComposeAccessMode(composer, ref left, ref right);

        // Feature Settings
        SetRowBounds(left, right, out left, out right);
        ComposeFeatureSettings(composer, left, right);

        // Save Button
        var controlRowBoundsRightFixed = ElementBounds.FixedSize(150, 30).WithFixedOffset(0, 25f).WithAlignment(EnumDialogArea.RightTop);
        composer
            .AddSmallButton(L("btnSaveChanges.Text"), OnSaveButtonPressed, controlRowBoundsRightFixed, EnumButtonStyle.Small, "btnSaveChanges");

        return composer;
    }

    public override void RefreshValues(GuiComposer composer)
    {
        composer.GetDropDown("cbxAccessMode")?.SetSelectedValue(Settings.Mode.ToString());
        var cbxWhitelistablePlayers = composer.GetDropDown("cbxWhitelistablePlayers");
        cbxWhitelistablePlayers?.SetSelectedIndex(0);
        _selectedWhitelistablePlayerId = cbxWhitelistablePlayers?.SelectedValue ?? string.Empty;

        var cbxWhitelistedPlayers = composer.GetDropDown("cbxWhitelistedPlayers");
        cbxWhitelistedPlayers?.SetSelectedIndex(0);
        _selectedWhitelistedPlayerId = cbxWhitelistedPlayers?.SelectedValue ?? string.Empty;

        var cbxBlacklistablePlayers = composer.GetDropDown("cbxBlacklistablePlayers");
        cbxBlacklistablePlayers?.SetSelectedIndex(0);
        _selectedBlacklistablePlayerId = cbxBlacklistablePlayers?.SelectedValue ?? string.Empty;

        var cbxBlacklistedPlayers = composer.GetDropDown("cbxBlacklistedPlayers");
        cbxBlacklistedPlayers?.SetSelectedIndex(0);
        _selectedBlacklistedPlayerId = cbxBlacklistedPlayers?.SelectedValue ?? string.Empty;
    }

    protected virtual void ComposeFeatureSettings(GuiComposer composer, ElementBounds left, ElementBounds right)
    {
        // INTENTIONALLY BLANK
    }

    private void ComposeAccessMode(GuiComposer composer, ref ElementBounds left, ref ElementBounds right)
    {
        var accessModes = Enum.GetNames<AccessMode>();
        var accessModeText = accessModes.Select(p => L($"cbxAccessMode.{p}")).ToArray();
        var selectedIndex = accessModes.IndexOf(Settings.Mode.ToString());

        composer
            .AddStaticText(L("lblAccessMode.Text"), LabelFont, EnumTextOrientation.Right, left, "lblAccessMode")
            .AddAutoSizeHoverText(L("lblAccessMode.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
            .AddDropDown(accessModes, accessModeText, selectedIndex, OnAccessModeChanged, right, "cbxAccessMode");

        if (Settings.Mode is AccessMode.Whitelist)
        {
            var whitelistedPlayers = Settings.Whitelist;
            var whitelistablePlayers = G.Capi.World.AllOnlinePlayers
                .Select(p => new Player(p.PlayerUID, p.PlayerName))
                .Except(whitelistedPlayers)
                .ToList();

            if (whitelistablePlayers.Count != 0)
            {
                SetRowBounds(left, right, out left, out right);
                var cbxWhitelistablePlayersBounds = right.FlatCopy().WithFixedWidth(260);
                var btnWhitelistablePlayerBounds = right.FlatCopy().WithFixedWidth(100).FixedRightOf(cbxWhitelistablePlayersBounds, 10);

                composer
                    .AddStaticText(L("lblWhitelistable.Text"), LabelFont, EnumTextOrientation.Right, left, "lblWhitelistable")
                    .AddAutoSizeHoverText(L("lblWhitelistable.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
                    .AddDropDown([.. whitelistablePlayers.Select(p => p.Id)], [.. whitelistablePlayers.Select(p => p.Name)], 0, OnSelectedWhitelistablePlayerChanged, cbxWhitelistablePlayersBounds, HoverTextFont, "cbxWhitelistablePlayers")
                    .AddSmallButton(L("btnAddWhitelistPlayer.Text"), OnAddWhitelistPlayer, btnWhitelistablePlayerBounds, EnumButtonStyle.Small, "btnAddWhitelistPlayer");
            }

            if (whitelistedPlayers.Count != 0)
            {
                SetRowBounds(left, right, out left, out right);
                var cbxWhitelistedPlayersBounds = right.FlatCopy().WithFixedWidth(260);
                var btnWhitelistedPlayerBounds = right.FlatCopy().WithFixedWidth(100).FixedRightOf(cbxWhitelistedPlayersBounds, 10);

                composer
                    .AddStaticText(L("lblWhitelisted.Text"), LabelFont, EnumTextOrientation.Right, left, "lblWhitelisted")
                    .AddAutoSizeHoverText(L("lblWhitelisted.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
                    .AddDropDown([.. whitelistedPlayers.Select(p => p.Id)], [.. whitelistedPlayers.Select(p => p.Name)], 0, OnSelectedWhitelistedPlayerChanged, cbxWhitelistedPlayersBounds, HoverTextFont, "cbxWhitelistedPlayers")
                    .AddSmallButton(L("btnRemoveWhitelistedPlayer.Text"), OnRemoveWhitelistedPlayer, btnWhitelistedPlayerBounds, EnumButtonStyle.Small, "btnRemoveWhitelistedPlayer");
            }
        }

        if (Settings.Mode is AccessMode.Blacklist)
        {
            var blacklistedPlayers = Settings.Blacklist;
            var blacklistablePlayers = G.Capi.World.AllOnlinePlayers
                .Select(p => new Player(p.PlayerUID, p.PlayerName))
                .Except(blacklistedPlayers)
                .ToList();

            if (blacklistablePlayers.Count != 0)
            {
                SetRowBounds(left, right, out left, out right);
                var cbxBlacklistablePlayersBounds = right.FlatCopy().WithFixedWidth(260);
                var btnBlacklistablePlayerBounds = right.FlatCopy().WithFixedWidth(100).FixedRightOf(cbxBlacklistablePlayersBounds, 10);

                composer
                    .AddStaticText(L("lblBlacklistable.Text"), LabelFont, EnumTextOrientation.Right, left, "lblBlacklistable")
                    .AddAutoSizeHoverText(L("lblBlacklistable.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
                    .AddDropDown([.. blacklistablePlayers.Select(p => p.Id)], [.. blacklistablePlayers.Select(p => p.Name)], 0, OnSelectedBlacklistablePlayerChanged, cbxBlacklistablePlayersBounds, HoverTextFont, "cbxBlacklistablePlayers")
                    .AddSmallButton(L("btnAddBlacklistPlayer.Text"), OnAddBlacklistPlayer, btnBlacklistablePlayerBounds, EnumButtonStyle.Small, "btnAddBlacklistPlayer");
            }

            if (blacklistedPlayers.Count != 0)
            {
                SetRowBounds(left, right, out left, out right);
                var cbxBlacklistedPlayersBounds = right.FlatCopy().WithFixedWidth(260);
                var btnBlacklistedPlayerBounds = right.FlatCopy().WithFixedWidth(100).FixedRightOf(cbxBlacklistedPlayersBounds, 10);

                composer
                    .AddStaticText(L("lblBlacklisted.Text"), LabelFont, EnumTextOrientation.Right, left, "lblBlacklisted")
                    .AddAutoSizeHoverText(L("lblBlacklisted.HoverText"), HoverTextFont, HOVER_TEXT_WIDTH, left)
                    .AddDropDown([.. blacklistedPlayers.Select(p => p.Id)], [.. blacklistedPlayers.Select(p => p.Name)], 0, OnSelectedBlacklistedPlayerChanged, cbxBlacklistedPlayersBounds, HoverTextFont, "cbxBlacklistedPlayers")
                    .AddSmallButton(L("btnRemoveBlacklistedPlayer.Text"), OnRemoveBlacklistPlayer, btnBlacklistedPlayerBounds, EnumButtonStyle.Small, "btnRemoveBlacklistedPlayer");
            }
        }
    }

    private void OnAccessModeChanged(string code, bool selected)
    {
        Settings.Mode = Enum.Parse<AccessMode>(code);
        Parent?.Recompose();
    }

    private void OnSelectedWhitelistablePlayerChanged(string playerId, bool selected)
    {
        _selectedWhitelistablePlayerId = playerId;
    }

    private bool OnAddWhitelistPlayer()
    {
        if (string.IsNullOrEmpty(_selectedWhitelistablePlayerId)) return false;
        var player = G.Capi.World.AllOnlinePlayers
            .Select(p => new Player(p.PlayerUID, p.PlayerName))
            .FirstOrDefault(p => p.Id == _selectedWhitelistablePlayerId);
        if (player is null) return false;
        Settings.Whitelist.Add(player);
        Parent?.Recompose();
        return true;
    }

    private void OnSelectedWhitelistedPlayerChanged(string playerId, bool selected)
    {
        _selectedWhitelistedPlayerId = playerId;
    }

    private bool OnRemoveWhitelistedPlayer()
    {
        Settings.Whitelist.RemoveAll(p => p.Id == _selectedWhitelistedPlayerId);
        Parent?.Recompose();
        return true;
    }

    private void OnSelectedBlacklistablePlayerChanged(string playerId, bool selected)
    {
        _selectedBlacklistablePlayerId = playerId;
    }

    private bool OnAddBlacklistPlayer()
    {
        if (string.IsNullOrEmpty(_selectedBlacklistablePlayerId)) return false;
        var player = G.Capi.World.AllOnlinePlayers
            .Select(p => new Player(p.PlayerUID, p.PlayerName))
            .FirstOrDefault(p => p.Id == _selectedBlacklistablePlayerId);
        if (player is null) return false;
        Settings.Blacklist.Add(player);
        Parent?.Recompose();
        return true;
    }

    private void OnSelectedBlacklistedPlayerChanged(string playerId, bool selected)
    {
        _selectedBlacklistedPlayerId = playerId;
    }

    private bool OnRemoveBlacklistPlayer()
    {
        Settings.Blacklist.RemoveAll(p => p.Id == _selectedBlacklistedPlayerId);
        Parent?.Recompose();
        return true;
    }

    private bool OnSaveButtonPressed()
    {
        ClientChannel?.SendPacket(Settings);
        return true;
    }
}