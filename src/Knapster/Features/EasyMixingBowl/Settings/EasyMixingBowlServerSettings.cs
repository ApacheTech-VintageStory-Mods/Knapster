using ProtoBuf;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.ChatCommands.DataStructures;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyMixingBowlServerSettings : FeatureSettings, IEasyXServerSettings<IEasyMixingBowlSettings>, IEasyMixingBowlSettings
{
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];

    /// <inheritdoc />
    public float SpeedMultiplier { get; set; } = 1f;

    /// <inheritdoc />
    public bool IncludeAutomated { get; set; } = false;
}