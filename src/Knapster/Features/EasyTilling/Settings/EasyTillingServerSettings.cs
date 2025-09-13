namespace Knapster.Features.EasyTilling.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyTillingServerSettings : FeatureSettings<EasyTillingServerSettings>, IEasyXServerSettings
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
    ///     Determines the multiplier to apply to the speed of Tilling with a hoe, when using the EasyTilling feature.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;
}