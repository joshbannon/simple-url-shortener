using System;

namespace LinkShrink
{
    public interface IShrinkStrategy
    {
        /// <summary>
        /// Returns a shortened relative path (does not contain the domain)
        /// </summary>
        /// <param name="fullUri"></param>
        /// <returns></returns>
        string GetShortPath(Uri fullUri);
    }
}
