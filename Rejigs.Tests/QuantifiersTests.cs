using NUnit.Framework;

namespace Rejigs.Tests;

public class QuantifiersTests
{
    [TestCase("prefix", true)]
    [TestCase("pre-fix", true)]
    [TestCase("pre--fix", true)]
    [TestCase("pre**fix", false)]
    public void ZeroOrMore_WithText_MatchesOptionalRepeatedText(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
            .Text("pre")
            .ZeroOrMore(r => r.Text("-"))
            .Text("fix")
            .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("AZ", true)]
    [TestCase("A1Z", true)]
    [TestCase("A123Z", true)]
    [TestCase("A1B2Z", false)]
    [TestCase("ABC", false)]
    public void ZeroOrMore_CombinedWithOtherPatterns(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
            .AtStart()
            .Text("A")
            .ZeroOrMore(r => r.AnyDigit())
            .Text("Z")
            .AtEnd()
            .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("aaa", true)]
    [TestCase("", false)]
    public void OneOrMore_MatchesRepeatedPattern(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().OneOrMore(r => r.Text("a")).Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("", true)]
    [TestCase("a", true)]
    [TestCase("aa", true)]
    public void Optional_MatchesOptionalPattern(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Optional(r => r.Text("a")).Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("colo", false)]
    [TestCase("colou", true)]
    [TestCase("colour", true)]
    [TestCase("colours", true)]
    public void Optional_WithText_MatchesOptionalText(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
            .Text("colou")
            .Optional("r")
            .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("fix", true)]
    [TestCase("pre-fix", true)]
    public void Optional_WithPattern_MatchesOptionalPattern(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
            .Optional(r => r.Text("pre-"))
            .Text("fix")
            .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("123", true)]
    [TestCase("12", false)]
    [TestCase("1234", false)]
    public void Exactly_MatchesExactCount(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AtStart().AnyDigit().Exactly(3).AtEnd().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("12", true)]
    [TestCase("123", true)]
    [TestCase("1", false)]
    public void AtLeast_MatchesMinimumCount(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyDigit().AtLeast(2).Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("12", true)]
    [TestCase("123", true)]
    [TestCase("1234", true)]
    [TestCase("1", false)]
    [TestCase("12345", true)]
    public void Between_MatchesRangeOfCounts(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyDigit().Between(2, 4).Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [Test]
    public void Exactly_ThrowsOnNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Rejigs.Create().Text("a").Exactly(-1));
    }

    [Test]
    public void AtLeast_ThrowsOnNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Rejigs.Create().Text("a").AtLeast(-5));
    }

    [Test]
    public void Between_ThrowsOnNegativeOrInvalidRange()
    {
        Assert.Throws<ArgumentException>(() => Rejigs.Create().Text("a").Between(-1, 2));
        Assert.Throws<ArgumentException>(() => Rejigs.Create().Text("a").Between(3, 2));
    }

    [Test]
    public void AnyInRange_ThrowsOnInvalidRange()
    {
        Assert.Throws<ArgumentException>(() => Rejigs.Create().AnyInRange('z', 'a'));
    }

    [TestCase("あ", true)]
    [TestCase("あああ", true)]
    [TestCase("", false)]
    public void Quantifiers_WithUnicodeCharacters(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().OneOrMore(r => r.Text("あ")).Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("", true)]
    [TestCase("x", true)]
    [TestCase("xx", true)]
    [TestCase("xxx", true)]
    public void ChainedQuantifiers_NestedGroups(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
            .ZeroOrMore(r => r.OneOrMore(rr => rr.Text("x")))
            .Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}