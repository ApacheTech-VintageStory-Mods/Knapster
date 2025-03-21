using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Settings;
using Gantry.Services.EasyX.Abstractions;
using Gantry.Services.EasyX.Extensions;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Knapster.Features.EasyMixingBowl.Systems;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class EasyMixingBowlServer : EasyXServerSystemBase<EasyMixingBowlServerSettings, EasyMixingBowlClientSettings, EasyMixingBowlSettings>
{
    protected override string SubCommandName => "MixingBowl";

    public override bool ShouldLoad(ICoreAPI api)
        => base.ShouldLoad(api)
        && api.ModLoader.IsModEnabled("aculinaryartillery");

    protected override void FeatureSpecificCommands(IChatCommand subCommand, CommandArgumentParsers parsers)
    {
        subCommand
            .WithDescription(LangEx.FeatureString("EasyMixingBowl", "Description"));

        subCommand
            .BeginSubCommand("speed")
            .WithAlias("s")
            .WithArgs(parsers.Float("multiplier"))
            .WithDescription(LangEx.FeatureString("EasyMixingBowl.SpeedMultiplier", "Description"))
            .HandleWith(OnChangeSpeedMultiplier)
            .EndSubCommand();

        subCommand
            .BeginSubCommand("automated")
            .WithAlias("a")
            .WithArgs(parsers.Bool("enabled"))
            .WithDescription(LangEx.FeatureString("EasyMixingBowl.IncludeAutomated", "Description"))
            .HandleWith(OnChangeIncludeAutomated)
            .EndSubCommand();
    }

    protected override void ExtraDisplayInfo(StringBuilder sb)
    {
        sb.AppendLine(LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier));
        sb.AppendLine(LangEx.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated));
    }

    private TextCommandResult OnChangeSpeedMultiplier(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<float?>() ?? 1f;
        Settings.SpeedMultiplier = GameMath.Clamp(value, 0f, 10f);
        var message = LangEx.FeatureString("Knapster", "SpeedMultiplier", SubCommandName, Settings.SpeedMultiplier);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }

    private TextCommandResult OnChangeIncludeAutomated(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<bool?>() ?? false;
        Settings.IncludeAutomated = value;
        var message = LangEx.FeatureString("Knapster", "IncludeAutomated", SubCommandName, Settings.IncludeAutomated);
        ServerChannel?.BroadcastUniquePacket(GeneratePacket);
        return TextCommandResult.Success(message);
    }
}