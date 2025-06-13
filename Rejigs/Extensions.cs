namespace Rejigs;

internal static class Extensions
{
    public static void Apply(this string value, Action<string> action)
    {
        action(value);
    }
}