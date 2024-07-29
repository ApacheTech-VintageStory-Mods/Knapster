using ProtoBuf;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.ChatCommands.DataStructures;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasySmithingServerSettings : FeatureSettings, IEasyXServerSettings<IEasySmithingSettings>, IEasySmithingSettings
{
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];

    /// <inheritdoc />
    public int CostPerClick { get; set; } = 1;

    /// <inheritdoc />
    public int VoxelsPerClick { get; set; } = 1;

    /// <inheritdoc />
    public bool InstantComplete { get; set; } = false;
}