namespace Knapster.Features.EasyClayForming.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyClayFormingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Clay Forming feature.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1)]   
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe.
    /// </summary>
    [ProtoMember(3)]
    public bool InstantComplete { get; set; } = false;
}