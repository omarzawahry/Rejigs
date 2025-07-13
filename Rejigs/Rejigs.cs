using System.Text;
using System.Text.RegularExpressions;

namespace Rejigs;

public partial class Rejigs
{
    private readonly StringBuilder _pattern = new();
    private RegexOptions _options = RegexOptions.None;

    /// <summary>
    /// Creates a new instance of the Rejigs regex builder.
    /// </summary>
    public static Rejigs Create() => new();

    private Rejigs Append(string value)
    {
        _pattern.Append(value);
        return this;
    }
}