using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Gets the constructed regex expression as a string.
    /// </summary>
    public string Expression => _pattern.ToString();

    /// <summary>
    /// Creates a new instance of the Rejigs regex builder for creating reusable fragments.
    /// </summary>
    public static Rejigs Fragment() => new();

    /// <summary>
    /// Uses a pre-built Rejigs fragment in the current pattern.
    /// </summary>
    /// <param name="fragment">The Rejigs fragment to include in the pattern.</param>
    /// <returns>The current Rejigs instance for chaining.</returns>
    public Rejigs Use(Rejigs fragment)
    {
        _pattern.Append(fragment._pattern.ToString());
        return this;
    }
    
    /// <summary>
    ///     Sets the regex options to ignore case when matching.
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs IgnoreCase()
    {
        _options |= RegexOptions.IgnoreCase;
        return this;
    }

    /// <summary>
    ///     Sets the regex options to compile the regex for faster execution.
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Compiled()
    {
        _options |= RegexOptions.Compiled;
        return this;
    }

    /// <summary>
    ///     Builds a Regex object with the current expression and options set in the builder.
    /// </summary>
    /// <returns>A Regex object.</returns>
    public Regex Build() => new(Expression, _options);

    /// <summary>
    ///     Builds a Regex object with the current expression and specified options.
    /// </summary>
    /// <param name="options">The options to use when building the Regex.</param>
    /// <returns>A Regex object.</returns>
    public Regex Build(RegexOptions options) => new(Expression, options);
}