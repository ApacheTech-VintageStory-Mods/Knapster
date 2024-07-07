using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyPressingPacket
{
    /// <summary>
    ///     Determines whether the EasyPressing feature should be used.
    /// </summary>
    public required bool Enabled { get; init; }

    /// <summary>
    ///     Initialises a new instance of the <see cref="EasyPressingPacket"/> class.
    /// </summary>
    public static EasyPressingPacket Create(bool enabled) => new()
    {
        Enabled = enabled
    };
}