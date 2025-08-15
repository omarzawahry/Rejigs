namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a regex pattern that represents an alternation (|).
    /// </summary>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Or() => Append("|");

    /// <summary>
    ///     Appends a regex pattern that matches either of the specified patterns.
    /// </summary>
    /// <param name="patterns">The patterns to match.</param>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Either(params Func<Rejigs, Rejigs>[] patterns)
    {
        if (patterns == null) throw new ArgumentNullException(nameof(patterns));
        if (patterns.Length == 0) return this;

        var result = "(?:";
        var first = true;

        foreach (var pattern in patterns)
        {
            if (pattern == null) throw new ArgumentException("Pattern cannot be null.", nameof(patterns));

            if (!first) result += "|";
            var patternContent = pattern(new Rejigs());
            result += patternContent._pattern;
            first = false;
        }

        result += ")";
        return new Rejigs(_pattern + result, _options);
    }
}