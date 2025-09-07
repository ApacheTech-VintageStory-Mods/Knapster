using System.ComponentModel;

namespace Knapster.Features.EasyTilling.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyTillingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the multiplier to apply to the speed of Tilling with a hoe, when using the EasyTilling feature.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;
}