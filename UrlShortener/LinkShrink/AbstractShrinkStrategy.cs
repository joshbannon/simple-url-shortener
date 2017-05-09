using System;
using System.Diagnostics;
using System.Text;

namespace LinkShrink
{
    public abstract class AbstractShrinkStrategy : IShrinkStrategy
    {
        protected readonly char[] AcceptableCharacters;
        protected readonly int AcceptableCharactersLength;
        protected readonly int MaxValue;

        protected AbstractShrinkStrategy(char[] acceptableCharacters, int maxPathLength)
        {
            AcceptableCharactersLength = acceptableCharacters.Length;
                
            MaxValue = (int)Math.Pow(AcceptableCharactersLength, maxPathLength);
            AcceptableCharacters = acceptableCharacters;
        }

        public abstract string GetShortPath(Uri fullUri);

        protected string ConvertValueToAcceptableCharsString(int value)
        {
            value = Math.Abs(value);
            if (value > MaxValue)
            {
                value %= MaxValue;
            }
            var sb = new StringBuilder(AcceptableCharacters.Length);
            do
            {
                sb.Append(AcceptableCharacters[value % AcceptableCharactersLength]);
                value = value / AcceptableCharactersLength;
            } while (value != 0);
            return sb.ToString();
        }
    }
}
