using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a regex pattern that matches any digit (\d).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyDigit() => Append(@"\d");

    /// <summary>
    ///     Appends a regex pattern that matches any non-digit (\D).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonDigit() => Append(@"\D");

    /// <summary>
    ///     Appends a regex pattern that matches any letter or digit (\w).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyLetterOrDigit() => Append(@"\w");

    /// <summary>
    ///     Appends a regex pattern that matches any non-letter or digit (\W).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonLetterOrDigit() => Append(@"\W");

    /// <summary>
    ///     Appends a regex pattern that matches any whitespace character (\s).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnySpace() => Append(@"\s");

    /// <summary>
    ///     Appends a regex pattern that matches any non-whitespace character (\S).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyNonSpace() => Append(@"\S");

    /// <summary>
    ///     Appends a regex pattern that matches any character (.).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyCharacter() => Append(".");

    /// <summary>
    ///     Appends a regex pattern that matches any one of the specified characters.
    /// </summary>
    /// <param name="characters">The characters to match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyOf(string characters) => Append($"[{Regex.Escape(characters)}]");

    /// <summary>
    ///     Appends a regex pattern that matches any character in the specified range.
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
    ///     Appends a regex pattern that matches any character except the specified characters.
    /// </summary>
    /// <param name="characters">The characters to exclude from the match.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AnyExcept(string characters) => Append($"[^{Regex.Escape(characters)}]");
}