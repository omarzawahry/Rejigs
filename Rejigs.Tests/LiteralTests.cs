using NUnit.Framework;

namespace Rejigs.Tests;

public class LiteralTests
{
    [TestCase("hello", true)]
    [TestCase("Hello", false)]
    public void Text_MatchesExactText(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("hello").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a.b*c+", true)]
    [TestCase("abc", false)]
    public void Text_EscapesSpecialCharacters(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("a.b*c+").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("", true)]
    [TestCase("a", false)]
    [TestCase("b", false)]
    [TestCase("     ", false)]
    public void Text_EmptyString_MatchesOnlyEmptyString(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase(" \t\n", true)]
    [TestCase(" ", false)]
    [TestCase("     \n", false)]
    public void Text_Whitespace_MatchesExactWhitespace(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text(" \t\n").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [Test]
    public void Text_AllRegexSpecialCharacters_AreEscaped()
    {
        var special = ".^$*+?()[]{}|\\";
        
        var regex = Rejigs.Create().Text(special).Build();
        Assert.That(special, Does.Match(regex));
        Assert.That(".", Does.Not.Match(regex));
    }

    [TestCase("こんにちは世界", true)]
    [TestCase("こんにちは", false)]
    public void Text_UnicodeCharacters_MatchExactly(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("こんにちは世界").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("foobar", true)]
    [TestCase("foo", false)]
    [TestCase("bar", false)]
    public void Text_MultipleCalls_ConcatenatesText(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("foo").Text("bar").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("123", true)]
    [TestCase("12", false)]
    [TestCase("abc", false)]
    public void Pattern_AddsRawRegexPattern(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Pattern(@"\d{3}").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("prefix-123-suffix", true)]
    [TestCase("prefix--suffix", false)]
    [TestCase("prefix-abc-suffix", false)]
    public void Pattern_CombinesWithOtherMethods(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
                          .Text("prefix-")
                          .Pattern(@"\d+")
                          .Text("-suffix")
                          .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("word", true)]
    [TestCase("two words", false)]
    [TestCase("word!", false)]
    public void Pattern_PreservesSpecialRegexCharacters(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
                          .AtStart()
                          .Pattern(@"\b\w+\b")
                          .AtEnd()
                          .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}