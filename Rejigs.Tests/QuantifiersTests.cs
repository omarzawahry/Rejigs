using NUnit.Framework;

namespace Rejigs.Tests;

public class QuantifiersTests
{
    [Test]
    public void ZeroOrMore_WithText_MatchesOptionalRepeatedText()
    {
        var regex = Rejigs.Create()
            .Text("pre")
            .ZeroOrMore(r => r.Text("-"))
            .Text("fix")
            .Build();

        Assert.That("prefix", Does.Match(regex));
        Assert.That("pre-fix", Does.Match(regex));
        Assert.That("pre--fix", Does.Match(regex));
        Assert.That("pre**fix", Does.Not.Match(regex));
    }

    [Test]
    public void ZeroOrMore_CombinedWithOtherPatterns()
    {
        var regex = Rejigs.Create()
            .AtStart()
            .Text("A")
            .ZeroOrMore(r => r.AnyDigit())
            .Text("Z")
            .AtEnd()
            .Build();

        Assert.That("AZ", Does.Match(regex));
        Assert.That("A1Z", Does.Match(regex));
        Assert.That("A123Z", Does.Match(regex));
        Assert.That("A1B2Z", Does.Not.Match(regex));
        Assert.That("ABC", Does.Not.Match(regex));
    }

    [Test]
    public void OneOrMore_MatchesRepeatedPattern()
    {
        var regex = Rejigs.Create().OneOrMore(r => r.Text("a")).Build();
        Assert.That("a", Does.Match(regex));
        Assert.That("aaa", Does.Match(regex));
        Assert.That("", Does.Not.Match(regex));
    }

    [Test]
    public void Optional_MatchesOptionalPattern()
    {
        var regex = Rejigs.Create().Optional(r => r.Text("a")).Build();
        Assert.That("", Does.Match(regex));
        Assert.That("a", Does.Match(regex));
        Assert.That("aa", Does.Match(regex));
    }

    [Test]
    public void Optional_WithText_MatchesOptionalText()
    {
        var regex = Rejigs.Create()
            .Text("colou")
            .Optional("r")
            .Build();

        Assert.That("colo", Does.Not.Match(regex));
        Assert.That("colou", Does.Match(regex));
        Assert.That("colour", Does.Match(regex));
        Assert.That("colours", Does.Match(regex));
    }

    [Test]
    public void Optional_WithPattern_MatchesOptionalPattern()
    {
        var regex = Rejigs.Create()
            .Optional(r => r.Text("pre-"))
            .Text("fix")
            .Build();

        Assert.That("fix", Does.Match(regex));
        Assert.That("pre-fix", Does.Match(regex));
    }

    [Test]
    public void Exactly_MatchesExactCount()
    {
        var regex = Rejigs.Create().AtStart().AnyDigit().Exactly(3).AtEnd().Build();
        Assert.That("123", Does.Match(regex));
        Assert.That("12", Does.Not.Match(regex));
        Assert.That("1234", Does.Not.Match(regex));
    }

    [Test]
    public void AtLeast_MatchesMinimumCount()
    {
        var regex = Rejigs.Create().AnyDigit().AtLeast(2).Build();
        Assert.That("12", Does.Match(regex));
        Assert.That("123", Does.Match(regex));
        Assert.That("1", Does.Not.Match(regex));
    }

    [Test]
    public void Between_MatchesRangeOfCounts()
    {
        var regex = Rejigs.Create().AnyDigit().Between(2, 4).Build();
        Assert.That("12", Does.Match(regex));
        Assert.That("123", Does.Match(regex));
        Assert.That("1234", Does.Match(regex));
        Assert.That("1", Does.Not.Match(regex));
        Assert.That("12345", Does.Match(regex));
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

    [Test]
    public void Quantifiers_WithUnicodeCharacters()
    {
        var regex = Rejigs.Create().OneOrMore(r => r.Text("あ")).Build();
        Assert.That("あ", Does.Match(regex));
        Assert.That("あああ", Does.Match(regex));
        Assert.That("", Does.Not.Match(regex));
    }

    [Test]
    public void ChainedQuantifiers_NestedGroups()
    {
        var regex = Rejigs.Create()
            .ZeroOrMore(r => r.OneOrMore(rr => rr.Text("x")))
            .Build();
        Assert.That("", Does.Match(regex));
        Assert.That("x", Does.Match(regex));
        Assert.That("xx", Does.Match(regex));
        Assert.That("xxx", Does.Match(regex));
    }
}