using Newtonsoft.Json.Converters;

namespace Knapster.Features.EasyPressing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasyPressingServerSettings : FeatureSettings<EasyPressingServerSettings>, IEasyXServerSettings
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
}