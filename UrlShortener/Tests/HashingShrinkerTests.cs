using System;
using LinkShrink;
using Xunit;

namespace Tests
{
    public class HashingShrinkerTests
    {
        private readonly char[] _saferCharacters = "0123456789ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();

        [Fact]
        public void UriMatchesAlsoMatchPath()
        {
            var uri1 = new Uri("http://google.com");
            var uri2 = new Uri("http://google.com");

            var shrinkStrategy = new HashingShrinkStrategy(_saferCharacters, 6);
            var short1 = shrinkStrategy.GetShortPath(uri1);
            var short2 = shrinkStrategy.GetShortPath(uri2);
            Assert.Equal(short1, short2);
        }

        [Fact]
        public void DifferingDoesNotMatch()
        {
            var uri1 = new Uri("http://google.com");
            var uri2 = new Uri("http://bing.com");

            var shrinkStrategy = new HashingShrinkStrategy(_saferCharacters, 6);
            var short1 = shrinkStrategy.GetShortPath(uri1);
            var short2 = shrinkStrategy.GetShortPath(uri2);
            Assert.NotEqual(short1, short2);
        }

        [Fact]
        public void SecureAndUnsecureDoesNotMatch()
        {
            var uri1 = new Uri("http://google.com");
            var uri2 = new Uri("https://google.com");

            var shrinkStrategy = new HashingShrinkStrategy(_saferCharacters, 6);
            var short1 = shrinkStrategy.GetShortPath(uri1);
            var short2 = shrinkStrategy.GetShortPath(uri2);
            Assert.NotEqual(short1, short2);
        }


    }
}
