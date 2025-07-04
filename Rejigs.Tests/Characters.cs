using NUnit.Framework;

namespace Rejigs.Tests;

public class Character
{
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
}