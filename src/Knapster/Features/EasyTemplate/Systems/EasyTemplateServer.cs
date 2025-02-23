using ApacheTech.VintageMods.Knapster.Features.EasyTemplate.Settings;
using Gantry.Services.EasyX.Abstractions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyTemplate.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class EasyTemplateServer : EasyXServerSystemBase<EasyTemplateServerSettings, EasyTemplateClientSettings, EasyTemplateSettings>
{
    protected override string SubCommandName => "Template";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(T("Description"));
    }
}