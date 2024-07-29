using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyHarvestingClientSettings : IEasyXClientSettings<IEasyHarvestingSettings>, IEasyHarvestingSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public float SpeedMultiplier { get; set; } = 1f;
}