using NUnit.Framework;

namespace Rejigs.Tests;

public class Quantifiers
{
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

}