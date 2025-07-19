using NUnit.Framework;

namespace Rejigs.Tests;

public class GroupingTests
{
    [TestCase("abcabc", true)]
    [TestCase("abc", false)]
    public void Grouping_WithQuantifier(string input, bool shouldMatch)
    {
        var regex = Rejigs.Create()
                          .Grouping(r => r.Text("abc"))
                          .Exactly(2)
                          .Build();

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}