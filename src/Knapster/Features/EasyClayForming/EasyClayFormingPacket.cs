using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyClayForming
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EasyClayFormingPacket
    {
        /// <summary>
        ///     Determines whether the EasyClayForming Feature should be used.
        /// </summary>
        public required bool Enabled { get; init; }

        /// <summary>
        ///     Determines the number of voxels that are handled at one time, when using the Easy Clay Forming feature.
        /// </summary>
        public required int VoxelsPerClick { get; init; }

        /// <summary>
        ///     Determines whether to instantly complete the current recipe, when using the Easy Clay Forming feature.
        /// </summary>
        public bool InstantComplete { get; init; }

        /// <summary>
        ///     Initialises a new instance of the <see cref="EasyClayFormingPacket"/> class.
        /// </summary>
        public static EasyClayFormingPacket Create(bool enabled, int voxelsPerClick, bool instantComplete)
        {
            return new EasyClayFormingPacket
            {
                Enabled = enabled, 
                VoxelsPerClick = voxelsPerClick,
                InstantComplete = instantComplete
            };
        }
    }
}