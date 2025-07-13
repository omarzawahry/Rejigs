using NUnit.Framework;

namespace Rejigs.Tests;

public class GroupingTests
{
    [Test]
    public void Grouping_WithQuantifier()
    {
        var regex = Rejigs.Create()
                          .Grouping(r => r.Text("abc"))
                          .Exactly(2)
                          .Build();

        Assert.That("abcabc", Does.Match(regex));
        Assert.That("abc", Does.Not.Match(regex));
    }
}