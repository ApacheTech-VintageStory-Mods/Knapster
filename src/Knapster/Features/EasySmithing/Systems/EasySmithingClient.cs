using ApacheTech.VintageMods.Knapster.Features.EasySmithing.Settings;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasySmithing.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasySmithingClient : EasyXClientSystemBase<EasySmithingClientSettings, IEasySmithingSettings>;