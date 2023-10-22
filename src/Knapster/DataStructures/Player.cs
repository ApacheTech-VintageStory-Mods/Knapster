namespace ApacheTech.VintageMods.Knapster.DataStructures
{
    /// <summary>
    ///     Represents a player that's been added to a whitelist, or blacklist.
    /// </summary>
    [JsonObject]
    public record Player(string Id, string Name);
}