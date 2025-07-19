using NUnit.Framework;

namespace Rejigs.Tests;

public class AlterationsTests
{
    [TestCase("cat", true)]
    [TestCase("dog", true)]
    [TestCase("mouse", true)]
    [TestCase("hamster", false)]
    public void Either_MatchesAlternatives(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
                          .Either(
                              r => r.Text("cat"),
                              r => r.Text("dog"),
                              r => r.Text("mouse")
                          ).Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("cat", true)]
    [TestCase("dog", true)]
    [TestCase("catdog", true)]
    public void Or_MatchesAlternatives(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
                          .Text("cat")
                          .Or()
                          .Text("dog")
                          .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}