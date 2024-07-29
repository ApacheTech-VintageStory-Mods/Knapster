using ApacheTech.VintageMods.Knapster.Features.EasyQuern.Settings;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyQuern.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyQuernClient : EasyXClientSystemBase<EasyQuernClientSettings, IEasyQuernSettings>;