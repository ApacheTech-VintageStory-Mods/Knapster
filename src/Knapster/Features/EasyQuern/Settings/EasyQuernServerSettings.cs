using Gantry.Core.GameContent.ChatCommands.DataStructures;
using ProtoBuf;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.FileSystem.Configuration.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyQuernServerSettings : EasyQuernSettings, IEasyXServerSettings
{ 
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];
}