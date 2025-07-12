using NUnit.Framework;

namespace Rejigs.Tests;

public class ComplexPatterns
{
    [Test]
    public void ValidatesEmailAddress()
    {
        var regex = Rejigs.Create()
                          .AtStart()
                          .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-_"))
                          .Text("@")
                          .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
                          .Text(".")
                          .AnyInRange('a', 'z')
                          .Between(2, 6)
                          .AtEnd()
                          .IgnoreCase()
                          .Build();

        Assert.That("user@example.com", Does.Match(regex));
        Assert.That("user.name@example.co.uk", Does.Match(regex));
        Assert.That("user.name@example.comaqwr", Does.Not.Match(regex));
        Assert.That("user@com", Does.Not.Match(regex));
        Assert.That("@example.com", Does.Not.Match(regex));
    }

    [Test]
    public void ValidatesPhoneNumber()
    {
        var regex = Rejigs.Create()
                          .Optional(r => r.Text("+"))
                          .Optional(r => r.AnyDigit().Between(1, 3).Text("-"))
                          .Grouping(r => r.AnyDigit().Exactly(3))
                          .Optional("-")
                          .Grouping(r => r.AnyDigit().Exactly(3))
                          .Optional("-")
                          .Grouping(r => r.AnyDigit().Exactly(4))
                          .Build();

        Assert.That("123-456-7890", Does.Match(regex));
        Assert.That("1234567890", Does.Match(regex));
        Assert.That("+1-123-456-7890", Does.Match(regex));
        Assert.That("abc-def-ghij", Does.Not.Match(regex));
    }

    [Test]
    public void ValidatesISODate()
    {
        var regex = Rejigs.Create()
                          .AtStart()
                          .AnyDigit().Exactly(4)
                          .Text("-")
                          .Either(
                              r => r.Text("0").AnyInRange('1', '9'),
                              r => r.Text("1").AnyInRange('0', '2')
                          )
                          .Text("-")
                          .Either(
                              r => r.Text("0").AnyInRange('1', '9'),
                              r => r.Either(
                                  r2 => r2.Text("1").AnyInRange('0', '9'),
                                  r2 => r2.Text("2").AnyInRange('0', '9'),
                                  r2 => r2.Text("3").AnyInRange('0', '1')
                              )
                          )
                          .AtEnd()
                          .Build();

        Assert.That("2023-01-01", Does.Match(regex));
        Assert.That("2023-12-31", Does.Match(regex));
        Assert.That("2023-13-01", Does.Not.Match(regex));
        Assert.That("2023-12-32", Does.Not.Match(regex));
        Assert.That("23-01-01", Does.Not.Match(regex));
    }

    [Test]
    public void ValidatesIPv4Address()
    {
        var octet = Rejigs.Create()
                          .Either(
                              r => r.AnyInRange('0', '9'),
                              r => r.AnyInRange('1', '9').AnyInRange('0', '9'),
                              r => r.Text("1").AnyInRange('0', '9').AnyInRange('0', '9'),
                              r => r.Text("2").Either(
                                  r2 => r2.AnyInRange('0', '4').AnyInRange('0', '9'),
                                  r2 => r2.Text("5").AnyInRange('0', '5')
                              )
                          );

        var regex = Rejigs.Create()
                          .AtStart()
                          .Grouping(r => octet)
                          .Text(".")
                          .Grouping(r => octet)
                          .Text(".")
                          .Grouping(r => octet)
                          .Text(".")
                          .Grouping(r => octet)
                          .AtEnd()
                          .Build();

        Assert.That("192.168.0.1", Does.Match(regex));
        Assert.That("127.0.0.1", Does.Match(regex));
        Assert.That("255.255.255.255", Does.Match(regex));
        Assert.That("256.0.0.1", Does.Not.Match(regex));
        Assert.That("192.168.0", Does.Not.Match(regex));
    }
}