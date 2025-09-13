namespace Knapster.Features.EasyPanning.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyPanningServerSettings : FeatureSettings<EasyPanningServerSettings>, IEasyXServerSettings
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
    ///     Determines the number of drops per layer when panning.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1)]
    public int DropsPerLayer { get; set; } = 1;

    /// <summary>
    ///     Determines the number of seconds per layer when panning.
    /// </summary>
    [ProtoMember(5)]
    [DefaultValue(4f)]
    public float SecondsPerLayer { get; set; } = 4f;

    /// <summary>
    ///     Determines the saturation per layer when panning.
    /// </summary>
    [ProtoMember(6)]
    [DefaultValue(3f)]
    public float SaturationPerLayer { get; set; } = 3f;
}