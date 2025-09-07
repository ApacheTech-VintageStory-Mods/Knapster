using System.ComponentModel;

namespace Knapster.Features.EasyDoughForming.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyDoughFormingServerSettings : FeatureSettings<EasyDoughFormingServerSettings>, IEasyXServerSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    [DefaultValue(AccessMode.Enabled)]
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    [ProtoMember(2)]
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    [ProtoMember(3)]
    public List<Player> Blacklist { get; set; } = [];

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Dough Forming feature.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(5)]
    public bool InstantComplete { get; set; } = false;
}