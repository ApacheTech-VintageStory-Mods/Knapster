using ProtoBuf;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.ChatCommands.DataStructures;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyPanningServerSettings : FeatureSettings, IEasyXServerSettings<IEasyPanningSettings>, IEasyPanningSettings
{
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];

    /// <inheritdoc />
    public float SecondsPerLayer { get; set; } = 4f;

    /// <inheritdoc />
    public int DropsPerLayer { get; set; } = 1;

    /// <inheritdoc />
    public float SaturationPerLayer { get; set; } = 3f;
}