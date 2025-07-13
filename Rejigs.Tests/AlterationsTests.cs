using NUnit.Framework;

namespace Rejigs.Tests;

public class AlterationsTests
{
    [Test]
    public void Either_MatchesAlternatives()
    {
        var regex = Rejigs.Create()
                          .Either(
                              r => r.Text("cat"),
                              r => r.Text("dog"),
                              r => r.Text("mouse")
                          ).Build();

        Assert.That("cat", Does.Match(regex));
        Assert.That("dog", Does.Match(regex));
        Assert.That("mouse", Does.Match(regex));
        Assert.That("hamster", Does.Not.Match(regex));
    }

    [Test]
    public void Or_MatchesAlternatives()
    {
        var regex = Rejigs.Create()
                          .Text("cat")
                          .Or()
                          .Text("dog")
                          .Build();

        Assert.That("cat", Does.Match(regex));
        Assert.That("dog", Does.Match(regex));
        Assert.That("catdog", Does.Match(regex));
    }
}