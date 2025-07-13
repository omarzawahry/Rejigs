namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a regex pattern that represents an alternation (|).
    /// </summary>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Or()
    {
        return Append("|");
    }

    /// <summary>
    ///     Appends a regex pattern that matches either of the specified patterns.
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
}