using System;

namespace LinkShrink
{
    public class HashingShrinkStrategy : AbstractShrinkStrategy
    {
        public HashingShrinkStrategy(char[] acceptableCharacters, int maxPathLength = 5) : base(acceptableCharacters, maxPathLength)
        {
            
        }

        public override string GetShortPath(Uri fullUri)
        {
            return ConvertValueToAcceptableCharsString(fullUri.GetHashCode());
        }
    }
}
