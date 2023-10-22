namespace ApacheTech.VintageMods.Knapster.Parsers.Extensions;

internal static class ParserExtensions
{
    internal static AccessModeParser AccessMode(this CommandArgumentParsers _)
        => new("mode", false);

    internal static FuzzyPlayerParser FuzzyPlayerSearch(this CommandArgumentParsers _)
        => new("search term", false);
}