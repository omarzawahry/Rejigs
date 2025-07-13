using NUnit.Framework;

namespace Rejigs.Tests;

public class LiteralTests
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
    public void Text_EmptyString_MatchesOnlyEmptyString()
    {
        var regex = Rejigs.Create().Text("").Build();
        
        Assert.That("", Does.Match(regex));
        Assert.That("a", Does.Not.Match(regex));
        Assert.That("b", Does.Not.Match(regex));
        Assert.That("     ", Does.Not.Match(regex));
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

    [Test]
    public void Pattern_AddsRawRegexPattern()
    {
        var regex = Rejigs.Create().Pattern(@"\d{3}").Build();
        
        Assert.That("123", Does.Match(regex));
        Assert.That("12", Does.Not.Match(regex));
        Assert.That("abc", Does.Not.Match(regex));
    }

    [Test]
    public void Pattern_CombinesWithOtherMethods()
    {
        var regex = Rejigs.Create()
                          .Text("prefix-")
                          .Pattern(@"\d+")
                          .Text("-suffix")
                          .Build();

        Assert.That("prefix-123-suffix", Does.Match(regex));
        Assert.That("prefix--suffix", Does.Not.Match(regex));
        Assert.That("prefix-abc-suffix", Does.Not.Match(regex));
    }

    [Test]
    public void Pattern_PreservesSpecialRegexCharacters()
    {
        var regex = Rejigs.Create()
                          .AtStart()
                          .Pattern(@"\b\w+\b")
                          .AtEnd()
                          .Build();

        Assert.That("word", Does.Match(regex));
        Assert.That("two words", Does.Not.Match(regex));
        Assert.That("word!", Does.Not.Match(regex));
    }
}