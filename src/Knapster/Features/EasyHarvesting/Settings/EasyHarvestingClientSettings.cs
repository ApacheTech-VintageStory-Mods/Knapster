namespace Knapster.Features.EasyHarvesting.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyHarvestingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the multiplier to apply to the speed of harvesting with a scythe, when using the EasyHarvesting feature.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;
}