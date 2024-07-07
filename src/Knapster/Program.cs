using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.ChatCommands.Parsers;
using ApacheTech.VintageMods.Knapster.ChatCommands.Parsers.Extensions;
using Gantry.Core.Extensions.Api;
using Gantry.Core.Hosting;
using Gantry.Services.FileSystem.Configuration;
using Gantry.Services.FileSystem.Enums;
using Gantry.Services.FileSystem.Hosting;
using Gantry.Services.HarmonyPatches.Hosting;
using Gantry.Services.Network.Hosting;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Knapster;

/// <summary>
///     Entry-point for the mod. This class will configure and build the IOC Container, and Service list for the rest of the mod.
///     
///     Registrations performed within this class should be global scope; by convention, features should aim to be as stand-alone as they can be.
/// </summary>
/// <remarks>
///     Only one derived instance of this class should be added to any single mod within
///     the VintageMods domain. This class will enable Dependency Injection, and add all
///     the domain services. Derived instances should only have minimal functionality, 
///     instantiating, and adding Application specific services to the IOC Container.
/// </remarks>
/// <seealso cref="ModHost" />
[UsedImplicitly]
internal sealed class Program() : ModHost(debugMode:
#if DEBUG
    true
#else
    false
#endif
)
{
    protected override void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
    {
        sapi.Logger.GantryDebug("[Knapster] Adding FileSystem Service");
        services.AddFileSystemService(sapi, o => o.RegisterSettingsFiles = true);
        services.AddFeatureGlobalSettings<ConfigurationSettings>();
    }

    protected override void ConfigureUniversalModServices(IServiceCollection services, ICoreAPI api)
    {
        api.Logger.GantryDebug("[Knapster] Adding Harmony Service");
        services.AddHarmonyPatchingService(api, o => o.AutoPatchModAssembly = true);

        api.Logger.GantryDebug("[Knapster] Adding Network Service");
        services.AddNetworkService(api);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.Logger.GantryDebug("[Knapster] Create Chat Command: knapster");

        var globalSettings = ModSettings.Global.Feature<ConfigurationSettings>();
        Scope = globalSettings.Scope;

        var command = api.ChatCommands.Create("knapster")
            .RequiresPrivilege(Privilege.controlserver)
            .WithDescription(LangEx.FeatureString("Knapster", "ServerCommandDescription"));

        command
            .BeginSubCommand("scope")
            .WithArgs(api.ChatCommands.Parsers.FileScope())
            .WithDescription(LangEx.FeatureString("Knapster.Scope", "Description"))
            .HandleWith(OnChangeSettingsScope)
            .EndSubCommand();
    }

    private TextCommandResult OnChangeSettingsScope(TextCommandCallingArgs args)
    {
        var parser = args.Parsers[0].To<FileScopeParser>();

        if (parser.IsMissing)
        {
            var message = LangEx.FeatureString("Knapster", "Scope", Scope.GetDescription());
            return TextCommandResult.Success(message);
        }

        if (parser.TryProcess(args) == EnumParseResult.Bad)
        {
            const string validScopes = "[W]orld | [G]lobal";
            var invalidScopeMessage = LangEx.FeatureString("Knapster", "InvalidScope", validScopes);
            return TextCommandResult.Error(invalidScopeMessage);
        }

        var scope = parser.Scope!;

        var globalSettings = ModSettings.Global.Feature<ConfigurationSettings>();
        if (globalSettings.Scope != scope)
        {
            ModSettings.CopyTo(scope.Value);
            globalSettings.Scope = Scope = scope.Value;
        }

        var scopeMessage = LangEx.FeatureString("Knapster", "SetScope", Scope.GetDescription());
        return TextCommandResult.Success(scopeMessage);
    }

    internal static FileScope Scope { get; private set; } = FileScope.World;
}