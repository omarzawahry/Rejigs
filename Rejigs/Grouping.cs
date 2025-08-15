namespace Rejigs;

public partial class Rejigs
{
    /// <summary>
    ///     Appends a non-capturing group to the regex pattern.
    /// </summary>
    /// <param name="pattern">The pattern to include in the non-capturing group.</param>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Grouping(Func<Rejigs, Rejigs> pattern)
    {
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));
        
        var groupContent = pattern(new Rejigs());
        return new Rejigs(_pattern + "(?:" + groupContent._pattern + ")", _options);
    }
    
    /// <summary>
    ///     Appends a capturing group to the regex pattern.
    /// </summary>
    /// <param name="pattern">The pattern to include in the capturing group.</param>
    /// <returns>A new Rejigs instance for chaining.</returns>
    public Rejigs Group(Func<Rejigs, Rejigs> pattern)
    {
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));
        
        var groupContent = pattern(new Rejigs());
        return new Rejigs(_pattern + "(" + groupContent._pattern + ")", _options);
    }
}