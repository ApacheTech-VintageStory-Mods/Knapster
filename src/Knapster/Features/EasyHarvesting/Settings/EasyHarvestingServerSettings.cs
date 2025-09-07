using System.ComponentModel;

namespace Knapster.Features.EasyHarvesting.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyHarvestingServerSettings : FeatureSettings<EasyHarvestingServerSettings>, IEasyXServerSettings
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
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyHarvesting feature.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;
}