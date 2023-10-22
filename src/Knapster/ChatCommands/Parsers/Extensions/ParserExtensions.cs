namespace ApacheTech.VintageMods.Knapster.ChatCommands.Parsers.Extensions;

/// <summary>
///     Extension methods that add new functionality to the <see cref="CommandArgumentParsers"/> class
/// </summary>
internal static class ParserExtensions
{
    /// <summary>
    ///     Parses a string as an <see cref="DataStructures.AccessMode"/> value, allowing partial matches.
    /// </summary>
    internal static AccessModeParser AccessMode(this CommandArgumentParsers _)
        => new("mode", false);

    /// <summary>
    ///     Allows the user to search for an online player, based on a partial match of their username.
    /// </summary>
    internal static FuzzyPlayerParser FuzzyPlayerSearch(this CommandArgumentParsers _)
        => new("search term", false);
}