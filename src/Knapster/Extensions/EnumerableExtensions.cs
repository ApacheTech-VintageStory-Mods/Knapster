namespace ApacheTech.VintageMods.Knapster.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsOneOf<T>(this T item, params T[] options) => options.Contains(item);
    }
}