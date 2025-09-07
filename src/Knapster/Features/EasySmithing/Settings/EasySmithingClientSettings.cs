using System.ComponentModel;

namespace Knapster.Features.EasySmithing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasySmithingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;

    /// <summary>
    ///     Determines the amount of durability that is lost at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(2)]
    [DefaultValue(1)]
    public int CostPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines the number of voxels that are handled at one time, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(3)]
    [DefaultValue(1)]
    public int VoxelsPerClick { get; set; } = 1;

    /// <summary>
    ///     Determines whether to instantly complete the current recipe, when using the Easy Smithing feature.
    /// </summary>
    [ProtoMember(4)]
    public bool InstantComplete { get; set; } = false;
}