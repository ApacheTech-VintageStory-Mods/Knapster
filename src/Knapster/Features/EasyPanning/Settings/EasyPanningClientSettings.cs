namespace Knapster.Features.EasyPanning.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyPanningClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the number of drops per layer when panning.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1)]
    public int DropsPerLayer { get; set; } = 1;

    /// <summary>
    ///     Determines the number of seconds per layer when panning.
    /// </summary>
    [ProtoMember(3)]
    [DefaultValue(1f)]
    public float SecondsPerLayer { get; set; } = 1f;

    /// <summary>
    ///     Determines the saturation per layer when panning.
    /// </summary>
    [ProtoMember(4)]
    [DefaultValue(1f)]
    public float SaturationPerLayer { get; set; } = 1f;
}