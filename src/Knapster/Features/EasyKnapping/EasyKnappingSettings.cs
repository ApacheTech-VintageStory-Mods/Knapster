using ProtoBuf;
using ApacheTech.VintageMods.Knapster.Abstractions;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using ApacheTech.VintageMods.Knapster.ChatCommands.DataStructures;

namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyKnappingSettings : FeatureSettings, IEasyFeatureSettings
{
    /// <summary>
    ///     Determines whether the EasyClayForming Feature should be used.
    /// </summary>
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <summary>
    ///     When the mode is set to `Whitelist`, only the players on this list will have the feature enabled.
    /// </summary>
    public List<Player> Whitelist { get; set; } = [];

    /// <summary>
    ///     When the mode is set to `Blacklist`, the players on this list will have the feature disabled.
    /// </summary>
    public List<Player> Blacklist { get; set; } = [];

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Knapping feature.
    /// </summary>
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    public bool InstantComplete { get; set; }
}