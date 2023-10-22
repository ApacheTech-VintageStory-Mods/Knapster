using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Knapster.DataStructures;
using ApacheTech.VintageMods.Knapster.Extensions;
using ApacheTech.VintageMods.Knapster.Parsers;
using ApacheTech.VintageMods.Knapster.Parsers.Extensions;
using Gantry.Core.DependencyInjection.Registration;
using Gantry.Services.FileSystem.Configuration;
using Gantry.Services.FileSystem.DependencyInjection;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.Knapster.Abstractions
{
    /// <summary>
    ///     Acts as a base class for all EasyX features, on the server.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <typeparam name="TPacket">The type of the packet.</typeparam>
    public abstract class FeatureServerSystemBase<TSettings, TPacket> : ServerModSystem, IServerServiceRegistrar
        where TSettings : class, IEasyFeatureSettings, new()
    {
        protected IServerNetworkChannel ServerChannel;
        internal static TSettings Settings { get; private set; } = new();

        /// <summary>
        /// Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="sapi">Access to the server-side API.</param>
        public void ConfigureServerModServices(IServiceCollection services, ICoreServerAPI sapi)
        {
            services.AddFeatureWorldSettings<TSettings>();
        }

        protected abstract string SubCommandName { get; }

        /// <summary>
        ///     Adds feature specific sub-commands to the feature command. 
        /// </summary>
        /// <param name="subCommand">The sub-command to add features to.</param>
        protected virtual void FeatureSpecificCommands(IChatCommand subCommand)
        {
            // Do nothing, by default.
        }

        /// <summary>
        ///     Full start to the mod on the server side
        /// <br /><br />In 1.17+ do NOT use this to add or update behaviors or attributes or other fixed properties of any block, item or entity, in code (additional to what is read from JSON).
        /// It is already too late to do that here, it will not be seen client-side.  Instead, code which needs to do that should be registered for event sapi.Event.AssetsFinalizers.  See VSSurvivalMod system BlockReinforcement.cs for an example.
        /// </summary>
        /// <param name="api"></param>
        public override void StartServerSide(ICoreServerAPI api)
        {
            Settings = ModSettings.World.Feature<TSettings>();
            var parsers = api.ChatCommands.Parsers;

            var command = api.ChatCommands.Get("knapster")
                .BeginSubCommand(SubCommandName.ToLowerInvariant())
                .WithAlias(SubCommandName[..1].ToLowerInvariant())
                .WithFeatureSpecifics(FeatureSpecificCommands)
                .HandleWith(DisplayInfo);

            command
                .BeginSubCommand("mode")
                .WithAlias("m")
                .WithArgs(parsers.AccessMode())
                .HandleWith(OnChangeMode)
                .EndSubCommand();

            command
                .BeginSubCommand("whitelist")
                .WithAlias("wl")
                .WithArgs(parsers.FuzzyPlayerSearch())
                .HandleWith(HandleWhitelist)
                .EndSubCommand();

            command
                .BeginSubCommand("blacklist")
                .WithAlias("bl")
                .WithArgs(parsers.FuzzyPlayerSearch())
                .HandleWith(HandleBlacklist)
                .EndSubCommand();

            command.EndSubCommand();

            ServerChannel = IOC.Services.Resolve<IServerNetworkService>()
                .DefaultServerChannel
                .RegisterMessageType<TPacket>();

            api.Event.PlayerJoin += player =>
            {
                ServerChannel.SendPacket(GeneratePacket(player), player);
            };
        }

        /// <summary>
        ///     Generates a packet, to send to the specified player.
        /// </summary>
        protected TPacket GeneratePacket(IPlayer player)
        {
            return GeneratePacketPerPlayer(player, IsEnabledFor(player));
        }

        /// <summary>
        ///     Generates a packet, to send to the specified player.
        /// </summary>
        protected abstract TPacket GeneratePacketPerPlayer(IPlayer player, bool isEnabled);

        /// <summary>
        ///     Determines whether this feature is enabled, for the specified player.
        /// </summary>
        internal static bool IsEnabledFor(IPlayer player)
        {
            return Settings.Mode switch
            {
                AccessMode.Disabled => false,
                AccessMode.Enabled => true,
                AccessMode.Whitelist => Settings.Whitelist.Any(p => p.Id == player.PlayerUID),
                AccessMode.Blacklist => Settings.Blacklist.All(p => p.Id != player.PlayerUID),
                _ => throw new ArgumentOutOfRangeException(nameof(player))
            };
        }

        /// <summary>
        ///     Call Handler: /knapster (X)
        /// </summary>
        protected virtual TextCommandResult DisplayInfo(TextCommandCallingArgs args)
        {
            var sb = new StringBuilder();
            sb.Append(LangEx.FeatureString("Knapster", "Mode", Lang.Get(SubCommandName.SplitPascalCase().UcFirst()), Settings.Mode));
            return TextCommandResult.Success(sb.ToString());
        }

        /// <summary>
        ///     Call Handler: /knapster (X) mode
        /// </summary>
        protected virtual TextCommandResult OnChangeMode(TextCommandCallingArgs args)
        {
            var mode = args.Parsers[0].To<AccessModeParser>().Mode;
            if (mode is null)
            {
                const string validModes = "[D]isabled | [E]nabled | [W]hitelist | [B]lacklist]";
                var invalidModeMessage = LangEx.FeatureString("Knapster", "InvalidMode", validModes);
                return TextCommandResult.Error(invalidModeMessage);
            }

            Settings.Mode = mode.Value;
            var modeMessage = LangEx.FeatureString("Knapster", "SetMode", SubCommandName, Settings.Mode);
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return TextCommandResult.Success(modeMessage);
        }

        /// <summary>
        ///     Call Handler: /knapster (X) whitelist
        /// </summary>
        protected virtual TextCommandResult HandleWhitelist(TextCommandCallingArgs args)
        {
            if (args.ArgCount > 0)
            {
                var message = AddRemovePlayerFromList(args, Settings.Whitelist, "Whitelist");
                return TextCommandResult.Success(message);
            }

            var sb = new StringBuilder();
            var resultCount = Settings.Whitelist.Count > 0 ? "Results" : "NoResults";
            sb.AppendLine(LangEx.FeatureString("Knapster", $"Whitelist.{resultCount}", SubCommandName));
            foreach (var p in Settings.Whitelist)
            {
                sb.AppendLine($" - {p.Name} (PID: {p.Id})");
            }
            return TextCommandResult.Success(sb.ToString());
        }

        /// <summary>
        ///     Call Handler: /knapster (X) blacklist
        /// </summary>
        protected virtual TextCommandResult HandleBlacklist(TextCommandCallingArgs args)
        {
            if (args.ArgCount > 0)
            {
                var message = AddRemovePlayerFromList(args, Settings.Blacklist, "Blacklist");
                return TextCommandResult.Success(message);
            }

            var sb = new StringBuilder();
            var resultCount = Settings.Blacklist.Count > 0 ? "Results" : "NoResults";
            sb.AppendLine(LangEx.FeatureString("Knapster", $"Blacklist.{resultCount}", SubCommandName));
            foreach (var p in Settings.Blacklist)
            {
                sb.AppendLine($" - {p.Name} (PID: {p.Id})");
            }
            return TextCommandResult.Success(sb.ToString());
        }

        private string AddRemovePlayerFromList(TextCommandCallingArgs args, ICollection<Player> list, string listType)
        {
            var parser = args.Parsers[0].To<FuzzyPlayerParser>();
            var players = parser.Results;
            var searchTerm = parser.Value;

            var message = players.Count switch
            {
                1 => FoundSinglePlayer(list, listType, players),
                > 1 => FoundMultiplePlayers(searchTerm, players),
                _ => FoundNoResults(searchTerm)
            };
            ServerChannel?.BroadcastUniquePacket(GeneratePacket);
            return message;
        }

        private string FoundSinglePlayer(ICollection<Player> list, string listType, List<IPlayer> players)
        {
            var result = players.First();
            var plr = list.SingleOrDefault(p => p.Id == result.PlayerUID);
            if (plr is not null)
            {
                list.Remove(plr);
                return LangEx.FeatureString("Knapster", $"{listType}.PlayerRemoved", result.PlayerName, SubCommandName);
            }

            list.Add(new Player(result.PlayerUID, result.PlayerName));
            ModSettings.World.Save(Settings);

            return LangEx.FeatureString("Knapster", $"{listType}.PlayerAdded", result.PlayerName, SubCommandName);
        }

        private static string FoundNoResults(string searchTerm)
            => LangEx.FeatureString("Knapster", "PlayerSearch.NoResults", searchTerm);

        private static string FoundMultiplePlayers(string searchTerm, List<IPlayer> players)
        {
            var sb = new StringBuilder();
            sb.Append(LangEx.FeatureString("Knapster", "PlayerSearch.MultipleResults", searchTerm));
            foreach (var p in players)
            {
                sb.Append($" - {p.PlayerName} (PID: {p.PlayerUID})");
            }
            return sb.ToString();
        }
    }
}