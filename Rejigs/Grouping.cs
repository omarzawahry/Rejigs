namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a non-capturing group to the regex pattern.
    /// </summary>
    /// <param name="pattern">The pattern to include in the non-capturing group.</param>
    /// <returns>The current Rejigs instance.</returns>
    public Rejigs Grouping(Func<Rejigs, Rejigs> pattern)
    {
        Append("(?:");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
    }
    
    private Rejigs Group(Func<Rejigs, Rejigs> pattern)
    {
        Append("(");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
    }
}