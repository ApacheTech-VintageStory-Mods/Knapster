using Newtonsoft.Json.Converters;

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
    [JsonConverter(typeof(StringEnumConverter))]
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
    [ChatCommand("speed", Aliases = ["s"], MinValue = 0.1f, MaxValue = 10f, DefaultValue = 1f)]
    public float SpeedMultiplier { get; set; } = 1f;
}