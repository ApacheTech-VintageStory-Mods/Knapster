using System.Globalization;

namespace ApacheTech.VintageMods.Knapster.ChatCommands.Parsers;

/// <summary>
///     Allows the user to search for an online player, based on a partial match of their username.
/// </summary>
/// <seealso cref="ArgumentParserBase" />
internal class FuzzyPlayerParser : ArgumentParserBase
{
    /// <summary>
    ///     Initialises a new instance of the <see cref="FuzzyPlayerParser"/> class.
    /// </summary>
    public FuzzyPlayerParser(string argName, bool isMandatoryArg) : base(argName, isMandatoryArg)
    {
    }

    /// <summary>
    ///     The list of players who's username matches the <see cref="Value"/> search term.
    /// </summary>
    public List<IPlayer> Results { get; private set; } = new();

    /// <summary>
    ///     The search term to filter the list of players with.
    /// </summary>
    public string Value { get; private set; }

    /// <inheritdoc />
    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null)
    {
        Value = args.RawArgs.PopWord();
        if (string.IsNullOrEmpty(Value)) return EnumParseResult.Bad;
        Results = FuzzyPlayerSearch(Value).ToList();
        return EnumParseResult.Good;
    }

    /// <inheritdoc />
    public override object GetValue() => Value;

    /// <inheritdoc />
    public override void SetValue(object data)
    {
        Value = data.ToString();
        if (string.IsNullOrEmpty(Value)) return;
        Results = FuzzyPlayerSearch(Value).ToList();
    }

    private static IEnumerable<IPlayer> FuzzyPlayerSearch(string searchTerm)
    {
        var onlineClients = ApiEx.ServerMain.PlayersByUid;
        if (onlineClients.TryGetValue(searchTerm, out var onlineClient)) return new List<IPlayer> { onlineClient };
        return onlineClients.Values
            .Where(client => client.PlayerName.StartsWith(searchTerm, true, CultureInfo.InvariantCulture));
    }
}