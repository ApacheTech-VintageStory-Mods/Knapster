using Gantry.Services.EasyX.Abstractions;
using ProtoBuf;

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class EasyMixingBowlClientSettings : IEasyXClientSettings<IEasyMixingBowlSettings>, IEasyMixingBowlSettings
{
    /// <inheritdoc />
    public bool Enabled { get; set; } = false;

    /// <inheritdoc />
    public float SpeedMultiplier { get; set; } = 1f;

    /// <inheritdoc />
    public bool IncludeAutomated { get; set; } = false;
}