using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Gets the constructed regex expression as a string.
    /// </summary>
    public string Expression => _pattern;

    /// <summary>
    /// Creates a new instance of the Rejigs regex builder for creating reusable fragments.
    /// </summary>
    public static Rejigs Fragment() => new();

    /// <summary>
    /// Uses a pre-built Rejigs fragment in the current pattern.
    /// </summary>
    /// <param name="fragment">The Rejigs fragment to include in the pattern.</param>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Use(Rejigs fragment)
    {
        if (fragment == null) throw new ArgumentNullException(nameof(fragment));
        return new Rejigs(_pattern + fragment._pattern, _options);
    }
    
    /// <summary>
    ///     Sets the regex options to ignore case when matching.
    /// </summary>
    /// <returns>A new Rejigs instance with updated options.</returns>
    public Rejigs IgnoreCase()
    {
        return new Rejigs(_pattern, _options | RegexOptions.IgnoreCase);
    }

    /// <summary>
    ///     Sets the regex options to compile the regex for faster execution.
    /// </summary>
    /// <returns>A new Rejigs instance with updated options.</returns>
    public Rejigs Compiled()
    {
        return new Rejigs(_pattern, _options | RegexOptions.Compiled);
    }

    /// <summary>
    ///     Builds a Regex object with the current expression and options set in the builder.
    /// </summary>
    /// <returns>A Regex object.</returns>
    /// <exception cref="ArgumentException">Thrown when the regex pattern is invalid.</exception>
    public Regex Build()
    {
        try
        {
            return new Regex(_pattern, _options);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Invalid regex pattern '{_pattern}': {ex.Message}", ex);
        }
    }

    /// <summary>
    ///     Builds a Regex object with the current expression and specified options.
    /// </summary>
    /// <param name="options">The options to use when building the Regex.</param>
    /// <returns>A Regex object.</returns>
    /// <exception cref="ArgumentException">Thrown when the regex pattern is invalid.</exception>
    public Regex Build(RegexOptions options)
    {
        try
        {
            return new Regex(_pattern, options);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Invalid regex pattern '{_pattern}': {ex.Message}", ex);
        }
    }
}