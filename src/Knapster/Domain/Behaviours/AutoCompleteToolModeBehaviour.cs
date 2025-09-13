using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Knapster.Domain.Behaviours;

/// <summary>
///     A collectible behaviour that provides an "AutoComplete" tool mode for certain items.
/// 
///     This behaviour registers a cached <see cref="SkillItem"/> representing the AutoComplete
///     tool mode during <see cref="OnLoaded"/>. The tool mode is exposed to the client via
///     <see cref="GetToolModes"/> when the underlying collectible object is a clay or a dough
///     item and the corresponding forming client mod settings are enabled.
/// </summary>
public class AutoCompleteToolModeBehaviour(CollectibleObject collectable) : CollectibleBehavior(collectable)
{
    private SkillItem? _autoComplete;
    private string? _featureName;

    /// <inheritdoc />
    public override void Initialize(JsonObject properties)
    {
        base.Initialize(properties);
        _featureName = properties["featureName"].AsString();
    }

    /// <inheritdoc />
    public override void OnLoaded(ICoreAPI api)
    {
        _autoComplete = ObjectCacheUtil.GetOrCreate(api, "autoComplete", () =>
        {
            var skillItem = new SkillItem
            {
                Code = new AssetLocation("auto"),
                Name = G.Lang.Translate("Knapster", "AutoComplete")
            };
            
            // Attach a client-side icon only when running on the client API.
            if (api is ICoreClientAPI capi)
            {
                skillItem.WithIcon(capi, capi.Gui.Icons.Drawfloodfill_svg);
            }

            return skillItem;
        });
    }

    /// <inheritdoc />
    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
    {
        if (_featureName is "ClayForming")
        {
            return G.Services.GetRequiredService<EasyClayFormingClient>().Settings.Enabled 
                ? [_autoComplete!] 
                : [];
        }

        if (_featureName is "DoughForming")
        {
            return G.Services.GetRequiredService<EasyDoughFormingClient>().Settings.Enabled 
                ? [_autoComplete!] 
                : [];
        }

        return [];
    }
}