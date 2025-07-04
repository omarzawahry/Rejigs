using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Rejigs.Tests
{
    public class RejigsTests
    {
       #region Group Tests

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

        #endregion

        #region Alternation Tests

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

        #endregion

        #region Complex Pattern Tests

        [Test]
        public void ValidatesEmailAddress()
        {
            var regex = Rejigs.Create()
                .AtStart()
                .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
                .Text("@")
                .OneOrMore(r => r.AnyLetterOrDigit().Or().AnyOf(".-"))
                .Text(".")
                .AnyLetterOrDigit()
                .AtLeast(2)
                .AtEnd()
                .Build();

            Assert.That("user@example.com", Does.Match(regex));
            Assert.That("user.name@example.co.uk", Does.Match(regex));
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

        #endregion

        #region Builder Tests

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

        #endregion
    }
}