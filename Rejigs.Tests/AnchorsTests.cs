using NUnit.Framework;

namespace Rejigs.Tests;

public class AnchorsTests
{
    [Test]
    public void AtStart_AnchorsBeginingOfLine()
    {
        var regex = Rejigs.Create().AtStart().Text("abc").Build();
        Assert.That("abc", Does.Match(regex));
        Assert.That("abcdef", Does.Match(regex));
        Assert.That("xabc", Does.Not.Match(regex));
    }

    [Test]
    public void AtEnd_AnchorsEndOfLine()
    {
        var regex = Rejigs.Create().Text("abc").AtEnd().Build();
        Assert.That("abc", Does.Match(regex));
        Assert.That("xabc", Does.Match(regex));
        Assert.That("abcx", Does.Not.Match(regex));
    }

    [Test]
    public void AtWordBoundary_MatchesAtWordBoundary()
    {
        var regex = Rejigs.Create().AtWordBoundary().Text("cat").Build();
        Assert.That("cat", Does.Match(regex));
        Assert.That("the cat", Does.Match(regex));
        Assert.That("tomcat", Does.Not.Match(regex));
    }

    [Test]
    public void NotAtWordBoundary_MatchesNotAtWordBoundary()
    {
        var regex = Rejigs.Create().NotAtWordBoundary().Text("cat").Build();
        Assert.That("tomcat", Does.Match(regex));
        Assert.That("cat", Does.Not.Match(regex));
    }
}