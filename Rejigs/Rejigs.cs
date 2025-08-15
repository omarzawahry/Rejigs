using System.Text;
using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    private readonly string _pattern;
    private readonly RegexOptions _options;

    /// <summary>
    /// Creates a new instance of the Rejigs regex builder.
    /// </summary>
    public static Rejigs Create() => new();

    // Private constructor for creating new instances
    private Rejigs() : this(string.Empty, RegexOptions.None) { }
    
    private Rejigs(string pattern, RegexOptions options)
    {
        _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        _options = options;
    }

    private Rejigs Append(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        return new Rejigs(_pattern + value, _options);
    }
}