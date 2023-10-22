using ApacheTech.VintageMods.Knapster.ChatCommands.DataStructures;

namespace ApacheTech.VintageMods.Knapster.Abstractions;

/// <summary>
///     Represents the base settings for each EasyX feature within this mod.
/// </summary>
public interface IEasyFeatureSettings
{
    /// <summary>
    ///     Determines whether the EasyClayForming Feature should be used.
    /// </summary>
    AccessMode Mode { get; set; }

    /// <summary>
    ///     When the mode is set to `Whitelist`, only the players on this list will have the feature enabled.
    /// </summary>
    List<Player> Whitelist { get; }

    /// <summary>
    ///     When the mode is set to `Blacklist`, the players on this list will have the feature disabled.
    /// </summary>
    List<Player> Blacklist { get; }
}