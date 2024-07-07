﻿using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.ChatCommands.DataStructures;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyPressingSettings : FeatureSettings, IEasyFeatureSettings
{
    /// <summary>
    ///     Determines whether the EasyPressing Feature should be used.
    /// </summary>
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <summary>
    ///     When the mode is set to `Whitelist`, only the players on this list will have the feature enabled.
    /// </summary>
    public List<Player> Whitelist { get; set; } = [];

    /// <summary>
    ///     When the mode is set to `Blacklist`, the players on this list will have the feature disabled.
    /// </summary>
    public List<Player> Blacklist { get; set; } = [];
}