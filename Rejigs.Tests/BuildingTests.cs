using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Rejigs.Tests;

public class BuildingTests
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

    [Test]
    public void IgnoreCase_SetsIgnoreCaseOptionAndMatchesCaseInsensitively()
    {
        var regex = Rejigs.Create()
                          .Text("hello")
                          .IgnoreCase()
                          .Build();
        
        Assert.That(regex.Options.HasFlag(RegexOptions.IgnoreCase), Is.True);
        Assert.That(regex.IsMatch("HELLO"), Is.True);
        Assert.That(regex.IsMatch("hello"), Is.True);
    }

    [Test]
    public void Compiled_SetsCompiledOption()
    {
        var regex = Rejigs.Create()
                          .Text("abc")
                          .Compiled()
                          .Build();
        
        Assert.That(regex.Options.HasFlag(RegexOptions.Compiled), Is.True);
    }

    [Test]
    public void CompiledAndIgnoreCase_SetsBothOptionsAndMatchesCaseInsensitively()
    {
        var regex = Rejigs.Create()
                           .Text("abc")
                           .Compiled()
                           .IgnoreCase()
                           .Build();
        
        Assert.That(regex.Options.HasFlag(RegexOptions.Compiled), Is.True);
        Assert.That(regex.Options.HasFlag(RegexOptions.IgnoreCase), Is.True);
        Assert.That(regex.IsMatch("ABC"), Is.True);
        Assert.That(regex.IsMatch("abc"), Is.True);
    }
}