using ApacheTech.VintageMods.Knapster.Abstractions;
using ApacheTech.VintageMods.Knapster.DataStructures;
using Gantry.Services.FileSystem.Configuration.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning
{
    /// <summary>
    ///     Represents user-controllable settings used for the mod.
    /// </summary>
    /// <seealso cref="FeatureSettings" />
    [JsonObject]
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EasyPanningSettings : FeatureSettings, IEasyFeatureSettings
    {
        /// <summary>
        ///     Determines whether the EasyPanning Feature should be used.
        /// </summary>
        public AccessMode Mode { get; set; } = AccessMode.Enabled;

        /// <summary>
        ///     When the mode is set to `Whitelist`, only the players on this list will have the feature enabled.
        /// </summary>
        public List<Player> Whitelist { get; set; } = new();

        /// <summary>
        ///     When the mode is set to `Blacklist`, the players on this list will have the feature disabled.
        /// </summary>
        public List<Player> Blacklist { get; set; } = new();

        /// <summary>
        ///     Determines the multiplier to apply to the speed of panning, when using the EasyPanning feature.
        /// </summary>
        public float SpeedMultiplier { get; set; } = 1f;

        /// <summary>
        ///     Determines the multiplier to apply to the amount of saturation consumed, when using the EasyPanning feature.
        /// </summary>
        public float SaturationMultiplier { get; set; } = 1f;
    }
}