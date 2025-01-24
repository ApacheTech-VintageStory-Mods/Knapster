﻿using Gantry.Core.GameContent.ChatCommands.DataStructures;
using ProtoBuf;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.FileSystem.Configuration.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming.Settings;

/// <summary>
///     Represents user-controllable settings used for the mod.
/// </summary>
/// <seealso cref="FeatureSettings{TSettings}" />
[JsonObject]
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyClayFormingServerSettings : EasyClayFormingSettings, IEasyXServerSettings
{
    /// <inheritdoc />
    public AccessMode Mode { get; set; } = AccessMode.Enabled;

    /// <inheritdoc />
    public List<Player> Whitelist { get; set; } = [];

    /// <inheritdoc />
    public List<Player> Blacklist { get; set; } = [];
}