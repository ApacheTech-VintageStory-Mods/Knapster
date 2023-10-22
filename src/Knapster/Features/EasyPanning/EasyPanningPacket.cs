using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPanning
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EasyPanningPacket
    {
        /// <summary>
        ///     Determines whether the EasyPanning feature should be used.
        /// </summary>
        public required bool Enabled { get; init; }

        /// <summary>
        ///     Determines the number of voxels that are handled at one time, when using the EasyPanning feature.
        /// </summary>
        public required float SpeedMultiplier { get; init; }

        /// <summary>
        ///     Determines the multiplier to apply to the amount of saturation consumed, when using the EasyPanning feature.
        /// </summary>
        public required float SaturationMultiplier { get; init; } = 1f;

        /// <summary>
        ///     Initialises a new instance of the <see cref="EasyPanningPacket"/> class.
        /// </summary>
        public static EasyPanningPacket Create(bool enabled, float speedMultiplier, float saturationMultiplier)
        {
            return new EasyPanningPacket
            {
                Enabled = enabled,
                SpeedMultiplier = speedMultiplier,
                SaturationMultiplier = saturationMultiplier
            };
        }
    }
}