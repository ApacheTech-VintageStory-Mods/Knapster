namespace Knapster.Features.EasyQuern.Settings;

[ProtoContract]
[ProtoInclude(100, typeof(EasyQuernClientSettings))]
public class EasyQuernSettings : FeatureSettings<EasyQuernServerSettings>
{
    /// <summary>
    ///     Determines the multiplier to apply to the time it takes to craft something in a quern.
    /// </summary>
    [ProtoMember(2)]
    public float SpeedMultiplier { get; set; } = 1f;

    /// <summary>
    ///     Determines whether to apply the speed multiplier to automated querns.
    /// </summary>
    [ProtoMember(3)]
    public bool IncludeAutomated { get; set; }

    /// <summary>
    ///     When active, the user will not need to keep the right-mouse button held down to keep the quern active.
    /// </summary>
    [ProtoMember(4)]
    public bool StickyMouseButton { get; set; }
}