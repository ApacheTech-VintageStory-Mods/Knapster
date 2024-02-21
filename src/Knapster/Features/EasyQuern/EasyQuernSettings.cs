using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.ChatCommands.DataStructures;
using Gantry.Services.FileSystem.Configuration.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
public class EasyQuernSettings : FeatureSettings, IEasyFeatureSettings
{
    /// <summary>
    ///     Determines whether the EasyQuern Feature should be used.
    /// </summary>
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <summary>
    ///     When the mode is set to `Whitelist`, only the players on this list will have the EasyQuern feature enabled.
    /// </summary>
    public List<Player> Whitelist { get; set; } = [];

    /// <summary>
    ///     When the mode is set to `Blacklist`, the players on this list will have the EasyQuern feature disabled.
    /// </summary>
    public List<Player> Blacklist { get; set; } = [];

    /// <summary>
    ///     Determines the multiplier to apply to the time it takes to craft something in a quern.
    /// </summary>
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    public bool IncludeAutomated { get; set; }
}