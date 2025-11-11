using Newtonsoft.Json.Converters;

namespace Knapster.Features.EasySmithing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract]
public class EasySmithingServerSettings : FeatureSettings<EasySmithingServerSettings>, IEasyXServerSettings
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
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1)]
    [ChatCommand("cost", Aliases = ["c"], MinValue = 1, MaxValue = 10, DefaultValue = 1)]
    public int CostPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(5)]
    [DefaultValue(1)]
    [ChatCommand("voxels", Aliases = ["v"], MinValue = 1, MaxValue = 8, DefaultValue = 1)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(6)]
    [ChatCommand("instant", Aliases = ["i"])]
    public bool InstantComplete { get; set; } = false;
}