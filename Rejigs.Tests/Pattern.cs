using NUnit.Framework;

namespace Rejigs.Tests;

public class Pattern
{
    [Test]
    public void Pattern_AddsRawRegexPattern()
    {
        var regex = Rejigs.Create().Pattern(@"\d{3}").Build();
        Assert.That("123", Does.Match(regex));
        Assert.That("12", Does.Not.Match(regex));
        Assert.That("abc", Does.Not.Match(regex));
    }

    [Test]
    public void Pattern_CombinesWithOtherMethods()
    {
        var regex = Rejigs.Create()
                          .Text("prefix-")
                          .Pattern(@"\d+")
                          .Text("-suffix")
                          .Build();

        Assert.That("prefix-123-suffix", Does.Match(regex));
        Assert.That("prefix--suffix", Does.Not.Match(regex));
        Assert.That("prefix-abc-suffix", Does.Not.Match(regex));
    }

    [Test]
    public void Pattern_PreservesSpecialRegexCharacters()
    {
        var regex = Rejigs.Create()
                          .AtStart()
                          .Pattern(@"\b\w+\b")
                          .AtEnd()
                          .Build();

        Assert.That("word", Does.Match(regex));
        Assert.That("two words", Does.Not.Match(regex));
        Assert.That("word!", Does.Not.Match(regex));
    }
}