namespace QueryFactory
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string name) =>
            char.ToLowerInvariant(name[0]) + name[1..];
    }
}
