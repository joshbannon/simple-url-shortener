using System;
using LinkShrink;
using Xunit;

namespace Tests
{
    public class RandomShrinkerTests
    {

        private readonly char[] _saferCharacters = "0123456789ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();

        [Fact]
        public void CanGetShortPath()
        {
            var shrinkStrategy = new RandomShrinkStrategy(_saferCharacters);
            var shortPath = shrinkStrategy.GetShortPath(new Uri("https://google.com"));

            Assert.False(string.IsNullOrWhiteSpace(shortPath));
        }

        [Fact]
        public void MultipleAttemptsResultInDifferntPaths()
        {
            var shrinkStrategy = new RandomShrinkStrategy(_saferCharacters);
            var shortPath = shrinkStrategy.GetShortPath(new Uri("https://google.com"));
            var shortPath2 = shrinkStrategy.GetShortPath(new Uri("https://google.com"));

            Assert.NotEqual(shortPath, shortPath2);
        }
    }
}
