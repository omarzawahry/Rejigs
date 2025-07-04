using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Rejigs.Tests;

public class Building
{
    [Test]
    public void Expression_ReturnsCorrectRegexString()
    {
        var builder = Rejigs.Create().AnyDigit().Exactly(3).Text("-").AnyDigit().Exactly(4);
        Assert.That(builder.Expression, Is.EqualTo(@"\d{3}-\d{4}"));
    }

    [Test]
    public void Build_WithOptions_SetsRegexOptions()
    {
        var regex = Rejigs.Create()
                          .Text("test")
                          .Build(RegexOptions.IgnoreCase);

        Assert.That(regex.Match("TEST").Success, Is.True);
        Assert.That(regex.Match("test").Success, Is.True);
    }

    [Test]
    public void Build_WithoutOptions_UsesDefaultOptions()
    {
        var regex = Rejigs.Create()
                          .Text("test")
                          .Build();

        Assert.That("test", Does.Match(regex));
        Assert.That("TEST", Does.Not.Match(regex));
    }
}