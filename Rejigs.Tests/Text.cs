using NUnit.Framework;

namespace Rejigs.Tests;

public class Text
{
    [Test]
    public void Text_MatchesExactText()
    {
        var regex = Rejigs.Create().Text("hello").Build();
        Assert.That("hello", Does.Match(regex));
        Assert.That("Hello", Does.Not.Match(regex));
    }

    [Test]
    public void Text_EscapesSpecialCharacters()
    {
        var regex = Rejigs.Create().Text("a.b*c+").Build();
        Assert.That("a.b*c+", Does.Match(regex));
        Assert.That("abc", Does.Not.Match(regex));
    }

    [Test]
    public void Text_EmptyString_MatchesAnything()
    {
        var regex = Rejigs.Create().Text("").Build();
        Assert.That("", Does.Match(regex));
        Assert.That("a", Does.Match(regex));
        Assert.That("b", Does.Match(regex));
        Assert.That("     ", Does.Match(regex));
    }

    [Test]
    public void Text_Whitespace_MatchesExactWhitespace()
    {
        var regex = Rejigs.Create().Text(" \t\n").Build();
        Assert.That(" \t\n", Does.Match(regex));
        Assert.That(" ", Does.Not.Match(regex));
        Assert.That("     \n", Does.Not.Match(regex));
    }

    [Test]
    public void Text_AllRegexSpecialCharacters_AreEscaped()
    {
        var special = ".^$*+?()[]{}|\\";
        var regex = Rejigs.Create().Text(special).Build();
        Assert.That(special, Does.Match(regex));
        Assert.That(".", Does.Not.Match(regex));
    }

    [Test]
    public void Text_UnicodeCharacters_MatchExactly()
    {
        var regex = Rejigs.Create().Text("こんにちは世界").Build();
        Assert.That("こんにちは世界", Does.Match(regex));
        Assert.That("こんにちは", Does.Not.Match(regex));
    }

    [Test]
    public void Text_MultipleCalls_ConcatenatesText()
    {
        var regex = Rejigs.Create().Text("foo").Text("bar").Build();
        Assert.That("foobar", Does.Match(regex));
        Assert.That("foo", Does.Not.Match(regex));
        Assert.That("bar", Does.Not.Match(regex));
    }
}