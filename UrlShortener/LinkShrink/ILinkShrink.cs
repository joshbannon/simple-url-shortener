using System;
using System.Threading.Tasks;

namespace LinkShrink
{
    public interface ILinkShrink
    {
        Task<Uri> Shrink(Uri longLink);

        Task<Uri> Shrink(Uri longLink, string shortPath);
    }
}
