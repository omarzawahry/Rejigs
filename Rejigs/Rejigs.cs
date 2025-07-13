using System.Text;
using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    private readonly StringBuilder _pattern = new();
    private RegexOptions _options = RegexOptions.None;

    /// <summary>
    /// Creates a new instance of the Rejigs regex builder.
    /// </summary>
    public static Rejigs Create() => new();

    /// <summary>
    /// Appends a regex pattern that matches the exact text provided. If the text is empty, matches only the empty string.
    /// </summary>
    /// <param name="text">The exact text to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Text(string text)
    {
        // Special-case: if text is empty, match only the empty string
        return Append(text == "" ? "^$" : Regex.Escape(text));
    }

    /// <summary>
    /// Appends a raw regex pattern fragment as-is.
    /// </summary>
    /// <param name="rawPattern">The raw regex pattern to append.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Pattern(string rawPattern) => Append(rawPattern);

    private Rejigs ZeroOrMore() => Append("*");
    private Rejigs OneOrMore() => Append("+");
    private Rejigs Optional() => Append("?");
    /// <summary>
    /// Appends a regex pattern that matches exactly the specified number of times.
    /// </summary>
    /// <param name="count">The exact number of times to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the count is negative.</exception>
    public Rejigs Exactly(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");
        return Append($"{{{count}}}");
    }
    /// <summary>
    /// Appends a regex pattern that matches at least the specified number of times.
    /// </summary>
    /// <param name="count">The minimum number of times to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the count is negative.</exception>
    public Rejigs AtLeast(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");
        return Append($"{{{count},}}" );
    }
    /// <summary>
    /// Appends a regex pattern that matches between the specified minimum and maximum number of times.
    /// </summary>
    /// <param name="min">The minimum number of times to match.</param>
    /// <param name="max">The maximum number of times to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the range is invalid.</exception>
    public Rejigs Between(int min, int max)
    {
        if (min < 0 || max < 0 || min > max)
            throw new ArgumentException($"Invalid range: {min}-{max}");
        return Append($"{{{min},{max}}}");
    }

    /// <summary>
    /// Appends a regex pattern that matches zero or more occurrences of the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match zero or more times.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs ZeroOrMore(Func<Rejigs, Rejigs> pattern) =>
        Group(pattern).ZeroOrMore();

    /// <summary>
    /// Appends a regex pattern that matches one or more occurrences of the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match one or more times.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs OneOrMore(Func<Rejigs, Rejigs> pattern) =>
        Group(pattern).OneOrMore();

    /// <summary>
    /// Appends a regex pattern that matches the specified text zero or one time.
    /// </summary>
    /// <param name="text">The text to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Optional(string text) => Text(text).Optional();

    /// <summary>
    /// Appends a regex pattern that matches the specified pattern zero or one time.
    /// </summary>
    /// <param name="pattern">The pattern to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Optional(Func<Rejigs, Rejigs> pattern) => Group(pattern).Optional();

    private Rejigs Append(string value)
    {
        _pattern.Append(value);
        return this;
    }
}