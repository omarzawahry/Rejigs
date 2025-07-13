using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a regex pattern that matches the exact text provided. If the text is empty,
    ///     matches only the empty string.
    /// </summary>
    /// <param name="text">The exact text to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Text(string text) =>
        // Special-case: if text is empty, match only the empty string
        Append(text == string.Empty ? "^$" : Regex.Escape(text));

    /// <summary>
    ///     Appends a raw regex pattern fragment as-is.
    /// </summary>
    /// <param name="rawPattern">The raw regex pattern to append.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Pattern(string rawPattern) => Append(rawPattern);
}