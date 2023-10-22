﻿using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyKnapping
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EasyKnappingPacket
    {
        /// <summary>
        ///     Determines whether the EasyKnapping feature should be used.
        /// </summary>
        public required bool Enabled { get; init; }

        /// <summary>
        ///     Determines the number of voxels that are handled at one time, when using the EasyKnapping feature.
        /// </summary>
        public required int VoxelsPerClick { get; init; }

        /// <summary>
        ///     Initialises a new instance of the <see cref="EasyKnappingPacket"/> class.
        /// </summary>
        public static EasyKnappingPacket Create(bool enabled, int voxelsPerClick)
        {
            return new EasyKnappingPacket
            {
                Enabled = enabled,
                VoxelsPerClick = voxelsPerClick
            };
        }
    }
}