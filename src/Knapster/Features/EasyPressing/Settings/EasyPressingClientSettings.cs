namespace Knapster.Features.EasyPressing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod (client-side).
/// </summary>
/// <seealso cref="IEasyXClientSettings" />
[JsonObject]
[ProtoContract]
public class EasyPressingClientSettings : IEasyXClientSettings
{
    /// <inheritdoc />
    [ProtoMember(1)]
    public bool Enabled { get; set; } = false;
}