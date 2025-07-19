using NUnit.Framework;

namespace Rejigs.Tests;

public class AnchorsTests
{
    [TestCase("abc", true)]
    [TestCase("abcdef", true)]
    [TestCase("xabc", false)]
    public void AtStart_AnchorsBeginingOfLine(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AtStart().Text("abc").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("abc", true)]
    [TestCase("xabc", true)]
    [TestCase("abcx", false)]
    public void AtEnd_AnchorsEndOfLine(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().Text("abc").AtEnd().Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("cat", true)]
    [TestCase("the cat", true)]
    [TestCase("tomcat", false)]
    public void AtWordBoundary_MatchesAtWordBoundary(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().AtWordBoundary().Text("cat").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("tomcat", true)]
    [TestCase("cat", false)]
    public void NotAtWordBoundary_MatchesNotAtWordBoundary(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create().NotAtWordBoundary().Text("cat").Build();
        
        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}