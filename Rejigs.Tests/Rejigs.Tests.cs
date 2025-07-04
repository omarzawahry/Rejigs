using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Rejigs.Tests
{
    public class RejigsTests
    {
        #region Pattern Tests

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

        #endregion

        #region Character Class Tests

        [Test]
        public void AnyDigit_MatchesSingleDigit()
        {
            var regex = Rejigs.Create().AnyDigit().Build();
            Assert.That("5", Does.Match(regex));
            Assert.That("a", Does.Not.Match(regex));
        }

        [Test]
        public void AnyNonDigit_MatchesSingleNonDigit()
        {
            var regex = Rejigs.Create().AnyNonDigit().Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("5", Does.Not.Match(regex));
        }

        [Test]
        public void AnyLetterOrDigit_MatchesWordCharacter()
        {
            var regex = Rejigs.Create().AnyLetterOrDigit().Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("_", Does.Match(regex));
            Assert.That("$", Does.Not.Match(regex));
        }

        [Test]
        public void AnyNonLetterOrDigit_MatchesNonWordCharacter()
        {
            var regex = Rejigs.Create().AnyNonLetterOrDigit().Build();
            Assert.That("$", Does.Match(regex));
            Assert.That("a", Does.Not.Match(regex));
        }

        [Test]
        public void AnySpace_MatchesWhitespace()
        {
            var regex = Rejigs.Create().AnySpace().Build();
            Assert.That(" ", Does.Match(regex));
            Assert.That("\t", Does.Match(regex));
            Assert.That("a", Does.Not.Match(regex));
        }

        [Test]
        public void AnyNonSpace_MatchesNonWhitespace()
        {
            var regex = Rejigs.Create().AnyNonSpace().Build();
            Assert.That("a", Does.Match(regex));
            Assert.That(" ", Does.Not.Match(regex));
        }

        [Test]
        public void AnyCharacter_MatchesAnyCharacter()
        {
            var regex = Rejigs.Create().AnyCharacter().Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("1", Does.Match(regex));
            Assert.That("$", Does.Match(regex));
            Assert.That("\n", Does.Not.Match(regex));
        }

        [Test]
        public void AnyOf_MatchesCharactersInSet()
        {
            var regex = Rejigs.Create().AnyOf("abc").Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("b", Does.Match(regex));
            Assert.That("c", Does.Match(regex));
            Assert.That("z", Does.Not.Match(regex));
        }

        [Test]
        public void AnyInRange_MatchesCharactersInRange()
        {
            var regex = Rejigs.Create().AnyInRange('a', 'f').Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("d", Does.Match(regex));
            Assert.That("f", Does.Match(regex));
            Assert.That("g", Does.Not.Match(regex));
        }

        [Test]
        public void AnyExcept_MatchesCharactersNotInSet()
        {
            var regex = Rejigs.Create().AnyExcept("xyz").Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("x", Does.Not.Match(regex));
            Assert.That("y", Does.Not.Match(regex));
        }

        #endregion

        #region Quantifier Tests

        [Test]
        public void ZeroOrMore_WithText_MatchesOptionalRepeatedText()
        {
            var regex = Rejigs.Create()
                .Text("pre")
                .ZeroOrMore(r => r.Text("-"))
                .Text("fix")
                .Build();

            Assert.That("prefix", Does.Match(regex));
            Assert.That("pre-fix", Does.Match(regex));
            Assert.That("pre--fix", Does.Match(regex));
            Assert.That("pre**fix", Does.Not.Match(regex));
        }

        [Test]
        public void ZeroOrMore_CombinedWithOtherPatterns()
        {
            var regex = Rejigs.Create()
                .AtStart()
                .Text("A")
                .ZeroOrMore(r => r.AnyDigit())
                .Text("Z")
                .AtEnd()
                .Build();

            Assert.That("AZ", Does.Match(regex));
            Assert.That("A1Z", Does.Match(regex));
            Assert.That("A123Z", Does.Match(regex));
            Assert.That("A1B2Z", Does.Not.Match(regex));
            Assert.That("ABC", Does.Not.Match(regex));
        }

        [Test]
        public void OneOrMore_MatchesRepeatedPattern()
        {
            var regex = Rejigs.Create().OneOrMore(r => r.Text("a")).Build();
            Assert.That("a", Does.Match(regex));
            Assert.That("aaa", Does.Match(regex));
            Assert.That("", Does.Not.Match(regex));
        }

        [Test]
        public void Optional_MatchesOptionalPattern()
        {
            var regex = Rejigs.Create().Optional(r => r.Text("a")).Build();
            Assert.That("", Does.Match(regex));
            Assert.That("a", Does.Match(regex));
            Assert.That("aa", Does.Match(regex));
        }

        [Test]
        public void Optional_WithText_MatchesOptionalText()
        {
            var regex = Rejigs.Create()
                .Text("colou")
                .Optional("r")
                .Build();

            Assert.That("colo", Does.Not.Match(regex));
            Assert.That("colou", Does.Match(regex));
            Assert.That("colour", Does.Match(regex));
            Assert.That("colours", Does.Match(regex));
        }

        [Test]
        public void Optional_WithPattern_MatchesOptionalPattern()
        {
            var regex = Rejigs.Create()
                .Optional(r => r.Text("pre-"))
                .Text("fix")
                .Build();

            Assert.That("fix", Does.Match(regex));
            Assert.That("pre-fix", Does.Match(regex));
        }

        [Test]
        public void Exactly_MatchesExactCount()
        {
            var regex = Rejigs.Create().AtStart().AnyDigit().Exactly(3).AtEnd().Build();
            Assert.That("123", Does.Match(regex));
            Assert.That("12", Does.Not.Match(regex));
            Assert.That("1234", Does.Not.Match(regex));
        }

        [Test]
        public void AtLeast_MatchesMinimumCount()
        {
            var regex = Rejigs.Create().AnyDigit().AtLeast(2).Build();
            Assert.That("12", Does.Match(regex));
            Assert.That("123", Does.Match(regex));
            Assert.That("1", Does.Not.Match(regex));
        }

        [Test]
        public void Between_MatchesRangeOfCounts()
        {
            var regex = Rejigs.Create().AnyDigit().Between(2, 4).Build();
            Assert.That("12", Does.Match(regex));
            Assert.That("123", Does.Match(regex));
            Assert.That("1234", Does.Match(regex));
            Assert.That("1", Does.Not.Match(regex));
            Assert.That("12345", Does.Match(regex));
        }

        #endregion

        #region Anchor Tests

        [Test]
        public void AtStart_AnchorsBeginingOfLine()
        {
            var regex = Rejigs.Create().AtStart().Text("abc").Build();
            Assert.That("abc", Does.Match(regex));
            Assert.That("abcdef", Does.Match(regex));
            Assert.That("xabc", Does.Not.Match(regex));
        }

        [Test]
        public void AtEnd_AnchorsEndOfLine()
        {
            var regex = Rejigs.Create().Text("abc").AtEnd().Build();
            Assert.That("abc", Does.Match(regex));
            Assert.That("xabc", Does.Match(regex));
            Assert.That("abcx", Does.Not.Match(regex));
        }

        [Test]
        public void AtWordBoundary_MatchesAtWordBoundary()
        {
            var regex = Rejigs.Create().AtWordBoundary().Text("cat").Build();
            Assert.That("cat", Does.Match(regex));
            Assert.That("the cat", Does.Match(regex));
            Assert.That("tomcat", Does.Not.Match(regex));
        }

        [Test]
        public void NotAtWordBoundary_MatchesNotAtWordBoundary()
        {
            var regex = Rejigs.Create().NotAtWordBoundary().Text("cat").Build();
            Assert.That("tomcat", Does.Match(regex));
            Assert.That("cat", Does.Not.Match(regex));
        }

        #endregion

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