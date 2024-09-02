using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Settings;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

namespace ApacheTech.VintageMods.Knapster.Features.EasyHarvesting.Systems;

[UsedImplicitly]
public sealed class EasyHarvestingServer : EasyXServerSystemBase<EasyHarvestingServerSettings, EasyHarvestingClientSettings, EasyHarvestingSettings>
{
    protected override string SubCommandName => "Harvesting";

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyHarvesting", "Description"));

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.OptionalFloat("speed"))
            .WithDescription(LangEx.FeatureString("EasyHarvesting.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.Append(LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 2f);
        var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}