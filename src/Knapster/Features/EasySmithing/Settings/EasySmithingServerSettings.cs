namespace Knapster.Features.EasySmithing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasySmithingServerSettings : EasySmithingSettings, IEasyXServerSettings
{
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];
}