using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyPressing.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyPressingClientSettings : IEasyXClientSettings<IEasyPressingSettings>, IEasyPressingSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;
}