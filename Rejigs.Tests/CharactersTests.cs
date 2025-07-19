using NUnit.Framework;

namespace Rejigs.Tests;

public class Character
{
    [TestCase("5", true)]
    [TestCase("a", false)]
    public void AnyDigit_MatchesSingleDigit(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyDigit().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("_", true)]
    [TestCase("$", true)]
    [TestCase("5", false)]
    public void AnyNonDigit_MatchesSingleNonDigit(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyNonDigit().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("_", true)]
    [TestCase("1", true)]
    [TestCase("$", false)]
    public void AnyLetterOrDigit_MatchesWordCharacter(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyLetterOrDigit().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("$", true)]
    [TestCase("a", false)]
    [TestCase("1", false)]
    public void AnyNonLetterOrDigit_MatchesNonWordCharacter(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyNonLetterOrDigit().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase(" ", true)]
    [TestCase("\t", true)]
    [TestCase("a", false)]
    public void AnySpace_MatchesWhitespace(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnySpace().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase(" ", false)]
    [TestCase("\t", false)]
    public void AnyNonSpace_MatchesNonWhitespace(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyNonSpace().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("1", true)]
    [TestCase("$", true)]
    [TestCase("\n", false)]
    public void AnyCharacter_MatchesAnyCharacter(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyCharacter().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("b", true)]
    [TestCase("c", true)]
    [TestCase("z", false)]
    public void AnyOf_MatchesCharactersInSet(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyOf("abc").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("d", true)]
    [TestCase("f", true)]
    [TestCase("af", true)]
    [TestCase("g", false)]
    public void AnyInRange_MatchesCharactersInRange(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyInRange('a', 'f').Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("a", true)]
    [TestCase("aq", true)]
    [TestCase("x", false)]
    [TestCase("y", false)]
    public void AnyExcept_MatchesCharactersNotInSet(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AnyExcept("xyz").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}