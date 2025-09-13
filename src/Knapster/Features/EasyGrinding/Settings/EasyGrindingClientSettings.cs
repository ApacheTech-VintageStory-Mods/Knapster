namespace Knapster.Features.EasyGrinding.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyGrindingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the multiplier to apply to the speed of grinding, when using the EasyGrinding feature.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1f)]
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    [ProtoMember(3)]
    public bool IncludeAutomated { get; set; } = false;

    /// <summary>
    ///     When active, the user will not need to keep the right-mouse button held down to keep the quern active.
    /// </summary>
    [ProtoMember(4)]
    public bool StickyMouseButton { get; set; } = false;
}