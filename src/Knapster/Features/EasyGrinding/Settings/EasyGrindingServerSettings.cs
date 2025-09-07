using System.ComponentModel;

namespace Knapster.Features.EasyGrinding.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyGrindingServerSettings : FeatureSettings<EasyGrindingServerSettings>, IEasyXServerSettings
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
    ///     Determines the multiplier to apply to the speed of grinding, when using the EasyGrinding feature.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    [ProtoMember(5)]
    public bool IncludeAutomated { get; set; } = false;

    /// <summary>
    ///     When active, the user will not need to keep the right-mouse button held down to keep the quern active.
    /// </summary>
    [ProtoMember(6)]
    public bool StickyMouseButton { get; set; } = false;
}