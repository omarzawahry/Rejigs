using System.Text;
using System.Text.RegularExpressions;

namespace Rejigs;

public class Rejigs
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
    
    /// <summary>
    /// Appends a regex anchor for the start of the string (^).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtStart() => Append("^");

    /// <summary>
    /// Appends a regex anchor for the end of the string ($).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtEnd() => Append("$");

    /// <summary>
    /// Appends a regex pattern that matches a word boundary (\b).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtWordBoundary() => Append(@"\b");

    /// <summary>
    /// Appends a regex pattern that matches a non-word boundary (\B).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs NotAtWordBoundary() => Append(@"\B");

    /// <summary>
    /// Appends a regex pattern that matches any digit (\d).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyDigit() => Append(@"\d");

    /// <summary>
    /// Appends a regex pattern that matches any non-digit (\D).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonDigit() => Append(@"\D");

    /// <summary>
    /// Appends a regex pattern that matches any letter or digit (\w).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyLetterOrDigit() => Append(@"\w");

    /// <summary>
    /// Appends a regex pattern that matches any non-letter or digit (\W).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonLetterOrDigit() => Append(@"\W");

    /// <summary>
    /// Appends a regex pattern that matches any whitespace character (\s).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnySpace() => Append(@"\s");

    /// <summary>
    /// Appends a regex pattern that matches any non-whitespace character (\S).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonSpace() => Append(@"\S");

    /// <summary>
    /// Appends a regex pattern that matches any character (.).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyCharacter() => Append(".");

    /// <summary>
    /// Appends a regex pattern that matches any one of the specified characters.
    /// </summary>
    /// <param name="characters">The characters to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyOf(string characters) => Append($"[{Regex.Escape(characters)}]");

    /// <summary>
    /// Appends a regex pattern that matches any character in the specified range.
    /// </summary>
    /// <param name="from">The starting character of the range.</param>
    /// <param name="to">The ending character of the range.</param>
    /// <returns>The current Rejigs instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the range is invalid.</exception>
    public Rejigs AnyInRange(char from, char to)
    {
        if (from > to)
            throw new ArgumentException($"Invalid range: '{from}'-'{to}'");
        return Append($"[{from}-{to}]");
    }

    /// <summary>
    /// Appends a regex pattern that matches any character except the specified characters.
    /// </summary>
    /// <param name="characters">The characters to exclude from the match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyExcept(string characters) => Append($"[^{Regex.Escape(characters)}]");

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

    private Rejigs Group(Func<Rejigs, Rejigs> pattern)
    {
        Append("(");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
    }

    /// <summary>
    /// Appends a non-capturing group to the regex pattern.
    /// </summary>
    /// <param name="pattern">The pattern to include in the non-capturing group.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Grouping(Func<Rejigs, Rejigs> pattern)
    {
        Append("(?:");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
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

    /// <summary>
    /// Appends a regex pattern that represents an alternation (|).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Or() => Append("|");

    /// <summary>
    /// Appends a regex pattern that matches either of the specified patterns.
    /// </summary>
    /// <param name="patterns">The patterns to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Either(params Func<Rejigs, Rejigs>[] patterns)
    {
        if (patterns.Length == 0) return this;

        Append("(?:");
        var first = true;

        foreach (var pattern in patterns)
        {
            if (!first) Append("|");
            pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
            first = false;
        }

        return Append(")");
    }

    /// <summary>
    /// Sets the regex options to ignore case when matching.
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs IgnoreCase()
    {
        _options |= RegexOptions.IgnoreCase;
        return this;
    }

    /// <summary>
    /// Sets the regex options to compile the regex for faster execution.
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Compiled()
    {
        _options |= RegexOptions.Compiled;
        return this;
    }

    /// <summary>
    /// Gets the constructed regex expression as a string.
    /// </summary>
    public string Expression => _pattern.ToString();

    /// <summary>
    /// Builds a Regex object with the current expression and options set in the builder.
    /// </summary>
    /// <returns>A Regex object.</returns>
    public Regex Build() => new(Expression, _options);

    /// <summary>
    /// Builds a Regex object with the current expression and specified options.
    /// </summary>
    /// <param name="options">The options to use when building the Regex.</param>
    /// <returns>A Regex object.</returns>
    public Regex Build(RegexOptions options) => new(Expression, options);

    private Rejigs Append(string value)
    {
        _pattern.Append(value);
        return this;
    }
}