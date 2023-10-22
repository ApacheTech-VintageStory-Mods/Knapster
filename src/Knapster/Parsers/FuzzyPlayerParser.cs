using System.Globalization;

namespace ApacheTech.VintageMods.Knapster.Parsers;

internal class FuzzyPlayerParser : ArgumentParserBase
{
    public FuzzyPlayerParser(string argName, bool isMandatoryArg) : base(argName, isMandatoryArg)
    {
    }

    public List<IPlayer> Results  { get; private set; } = new();

    public string Value { get; private set; }

    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null)
    {
        Value = args.RawArgs.PopWord();
        if (string.IsNullOrEmpty(Value)) return EnumParseResult.Bad;
        Results = FuzzyPlayerSearch(Value).ToList();
        return EnumParseResult.Good;
    }

    public override object GetValue() => Value;

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