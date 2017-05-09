using System;

namespace LinkShrink
{
    public class RandomShrinkStrategy : AbstractShrinkStrategy
    {
        public RandomShrinkStrategy(char[] acceptableCharacters, int maxPathLength = 5) : base(acceptableCharacters, maxPathLength)
        {
            
        }

        private readonly Random _random = new Random();

        public override string GetShortPath(Uri fullUri)
        {
            return ConvertValueToAcceptableCharsString(_random.Next(MaxValue));
        }
    }
}
