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

    [Test]
    public void Fragment_CreatesReusablePatternComponent()
    {
        var fragment = Rejigs.Fragment()
                                   .AnyDigit()
                                   .Exactly(3);
        
        Assert.That(fragment.Expression, Is.EqualTo(@"\d{3}"));
    }

    [Test]
    public void Use_IncorporatesFragmentIntoPattern()
    {
        var digitFragment = Rejigs.Fragment()
                                        .AnyDigit()
                                        .Exactly(3);
        
        var fullPattern = Rejigs.Create()
                                      .Use(digitFragment)
                                      .Text("-")
                                      .Use(digitFragment);
        
        Assert.That(fullPattern.Expression, Is.EqualTo(@"\d{3}-\d{3}"));
    }

    [Test]
    public void Use_WithMultipleFragments_CombinesCorrectly()
    {
        var letterFragment = Rejigs.Fragment()
                                         .AnyInRange('a', 'z')
                                         .OneOrMore(r => r.AnyInRange('a', 'z'));
        
        var digitFragment = Rejigs.Fragment()
                                        .AnyDigit()
                                        .Exactly(2);
        
        var pattern = Rejigs.Create()
                                  .Use(letterFragment)
                                  .Text("-")
                                  .Use(digitFragment)
                                  .Text("-")
                                  .Use(letterFragment);

        Assert.That(pattern.Expression,
            Is.EqualTo(@"[a-z]([a-z])+-\d{2}-[a-z]([a-z])+"));
    }

    [Test]
    public void Use_FragmentWithRegexMatching_WorksCorrectly()
    {
        var domainFragment = Rejigs.Fragment()
                                         .AnyLetterOrDigit()
                                         .OneOrMore(r => r.AnyLetterOrDigit())
                                         .Optional(".");
        
        var emailPattern = Rejigs.Create()
                                       .Use(domainFragment)
                                       .Text("@")
                                       .Use(domainFragment)
                                       .Text(".")
                                       .AnyInRange('a', 'z')
                                       .Between(2, 4);
        
        var regex = emailPattern.Build();
        
        Assert.That(regex.IsMatch("test@example.com"), Is.True);
        Assert.That(regex.IsMatch("user123@domain.org"), Is.True);
        Assert.That(regex.IsMatch("invalid-email"), Is.False);
    }

    [Test]
    public void Fragment_CanBeReusedMultipleTimes()
    {
        var wordFragment = Rejigs.Fragment()
                                       .AnyInRange('a', 'z')
                                       .AtLeast(1);
        
        var pattern1 = Rejigs.Create()
                                   .Use(wordFragment)
                                   .AnySpace()
                                   .Use(wordFragment);
        
        var pattern2 = Rejigs.Create()
                                   .Text("Start:")
                                   .AnySpace()
                                   .Use(wordFragment)
                                   .AnySpace()
                                   .Text("End");
        
        Assert.That(pattern1.Expression, Is.EqualTo(@"[a-z]{1,}\s[a-z]{1,}"));
        Assert.That(pattern2.Expression, Is.EqualTo(@"Start:\s[a-z]{1,}\sEnd"));
        
        var regex1 = pattern1.Build();
        var regex2 = pattern2.Build();
        
        Assert.That(regex1.IsMatch("hello world"), Is.True);
        Assert.That(regex1.IsMatch("a b"), Is.True);
        Assert.That(regex1.IsMatch("123 456"), Is.False);
        
        Assert.That(regex2.IsMatch("Start: hello End"), Is.True);
        Assert.That(regex2.IsMatch("Start: test End"), Is.True);
        Assert.That(regex2.IsMatch("Start: 123 End"), Is.False);
    }

    [Test]
    public void Fragment_WithComplexPattern_MaintainsStructure()
    {
        var phoneFragment = Rejigs.Fragment()
                                        .Grouping(g => g.AnyDigit().Exactly(3))
                                        .Text("-")
                                        .Grouping(g => g.AnyDigit().Exactly(3))
                                        .Text("-")
                                        .Grouping(g => g.AnyDigit().Exactly(4));
        
        var pattern = Rejigs.Create()
                                  .Text("Phone: ")
                                  .Use(phoneFragment);
        
        var regex = pattern.Build();

        Assert.That(regex.IsMatch("Phone: 123-456-7890"), Is.True);
        Assert.That(regex.IsMatch("Phone: 12-345-6789"), Is.False);
        Assert.That(regex.IsMatch("Phone: 123-456-78901"),
                    Is.True); // Should match because there's no anchor at the end
    }

    [Test]
    public void Fragment_WithAnchors_WorksCorrectly()
    {
        // Test anchors in the main pattern, not in fragments
        var digitGroup = Rejigs.Fragment()
                                     .Grouping(g => g.AnyDigit().Exactly(3));
        
        var anchoredPattern = Rejigs.Create()
                                          .AtStart()
                                          .Use(digitGroup)
                                          .Text("-")
                                          .Use(digitGroup)
                                          .Text("-")
                                          .Grouping(g => g.AnyDigit().Exactly(4))
                                          .AtEnd();

        var regex = anchoredPattern.Build();

        // This should only match exact patterns due to anchors
        Assert.That(regex.IsMatch("123-456-7890"), Is.True);
        Assert.That(regex.IsMatch("12-345-6789"), Is.False);
        Assert.That(regex.IsMatch("Phone: 123-456-7890"),
                    Is.False); // Should not match due to prefix
    }
}