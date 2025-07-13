namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a regex anchor for the start of the string (^).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtStart() => Append("^");

    /// <summary>
    ///     Appends a regex anchor for the end of the string ($).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtEnd() => Append("$");

    /// <summary>
    ///     Appends a regex pattern that matches a word boundary (\b).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs AtWordBoundary() => Append(@"\b");

    /// <summary>
    ///     Appends a regex pattern that matches a non-word boundary (\B).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs NotAtWordBoundary() => Append(@"\B");
}