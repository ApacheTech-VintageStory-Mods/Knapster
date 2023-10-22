using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.ChatCommands.DataStructures;
using Gantry.Services.FileSystem.Configuration.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
public class EasySmithingSettings : FeatureSettings, IEasyFeatureSettings
{
    /// <summary>
    ///     Determines whether the EasyClayForming Feature should be used.
    /// </summary>
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <summary>
    ///     When the mode is set to `Whitelist`, only the players on this list will have the feature enabled.
    /// </summary>
    public List<Player> Whitelist { get; set; } = new();

    /// <summary>
    ///     When the mode is set to `Blacklist`, the players on this list will have the feature disabled.
    /// </summary>
    public List<Player> Blacklist { get; set; } = new();

    /// <summary>
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    public int CostPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Smithing feature.
    /// </summary>
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Smithing feature.
    /// </summary>
    public bool InstantComplete { get; set; }
}