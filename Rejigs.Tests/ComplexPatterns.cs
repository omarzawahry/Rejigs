using NUnit.Framework;

namespace Rejigs.Tests;

public class ComplexPatterns
{
    [TestCase("user@example.com", true)]
    [TestCase("user.name@example.co.uk", true)]
    [TestCase("user.name@example.comaqwr", false)]
    [TestCase("user@com", false)]
    [TestCase("@example.com", false)]
    public void ValidatesEmailAddress(string input, bool shouldMatch)
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

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("123-456-7890", true)]
    [TestCase("1234567890", true)]
    [TestCase("+1-123-456-7890", true)]
    [TestCase("abc-def-ghij", false)]
    public void ValidatesPhoneNumber(string input, bool shouldMatch)
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

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("2023-01-01", true)]
    [TestCase("2023-12-31", true)]
    [TestCase("2023-13-01", false)]
    [TestCase("2023-12-32", false)]
    [TestCase("23-01-01", false)]
    public void ValidatesISODate(string input, bool shouldMatch)
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

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }

    [TestCase("192.168.0.1", true)]
    [TestCase("127.0.0.1", true)]
    [TestCase("255.255.255.255", true)]
    [TestCase("256.0.0.1", false)]
    [TestCase("192.168.0", false)]
    public void ValidatesIPv4Address(string input, bool shouldMatch)
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

        if (shouldMatch)
            Assert.That(input, Does.Match(regex));
        else
            Assert.That(input, Does.Not.Match(regex));
    }
}