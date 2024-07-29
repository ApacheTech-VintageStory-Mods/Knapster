using ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;
using Gantry.Core.Hosting;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyHarvestingClient : EasyXClientSystemBase<EasyHarvestingClientSettings, IEasyHarvestingSettings>;