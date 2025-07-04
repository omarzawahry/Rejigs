using System.Text;
using System.Text.RegularExpressions;

namespace Rejigs;

public class Rejigs
{
    private readonly StringBuilder _pattern = new();

    public static Rejigs Create() => new();

    public Rejigs Text(string text) => Append(Regex.Escape(text));
    public Rejigs Pattern(string rawPattern) => Append(rawPattern);
    
    public Rejigs AtStart() => Append("^");
    public Rejigs AtEnd() => Append("$");
    public Rejigs AtWordBoundary() => Append(@"\b");
    public Rejigs NotAtWordBoundary() => Append(@"\B");

    public Rejigs AnyDigit() => Append(@"\d");
    public Rejigs AnyNonDigit() => Append(@"\D");
    public Rejigs AnyLetterOrDigit() => Append(@"\w");
    public Rejigs AnyNonLetterOrDigit() => Append(@"\W");
    public Rejigs AnySpace() => Append(@"\s");
    public Rejigs AnyNonSpace() => Append(@"\S");
    public Rejigs AnyCharacter() => Append(".");
    public Rejigs AnyOf(string characters) => Append($"[{Regex.Escape(characters)}]");
    public Rejigs AnyInRange(char from, char to) => Append($"[{from}-{to}]");
    public Rejigs AnyExcept(string characters) => Append($"[^{Regex.Escape(characters)}]");

    private Rejigs ZeroOrMore() => Append("*");
    private Rejigs OneOrMore() => Append("+");
    private Rejigs Optional() => Append("?");
    public Rejigs Exactly(int count) => Append($"{{{count}}}");
    public Rejigs AtLeast(int count) => Append($"{{{count},}}");
    public Rejigs Between(int min, int max) => Append($"{{{min},{max}}}");

    private Rejigs Group(Func<Rejigs, Rejigs> pattern)
    {
        Append("(");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
    }

    public Rejigs Grouping(Func<Rejigs, Rejigs> pattern)
    {
        Append("(?:");
        pattern(new Rejigs())._pattern.ToString().Apply(p => Append(p));
        return Append(")");
    }

    public Rejigs ZeroOrMore(Func<Rejigs, Rejigs> pattern) =>
        Group(pattern).ZeroOrMore();

    public Rejigs OneOrMore(Func<Rejigs, Rejigs> pattern) =>
        Group(pattern).OneOrMore();

    public Rejigs Optional(string text) => Text(text).Optional();

    public Rejigs Optional(Func<Rejigs, Rejigs> pattern) => Group(pattern).Optional();

    public Rejigs Or() => Append("|");

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

    public string Expression => _pattern.ToString();
    public Regex Build() => new(Expression);
    public Regex Build(RegexOptions options) => new(Expression, options);

    private Rejigs Append(string value)
    {
        _pattern.Append(value);
        return this;
    }
}