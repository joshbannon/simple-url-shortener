using System;
using System.Threading.Tasks;
using LinkShrink;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class EntryController : Controller
    {
        private readonly ILinkShrink _linkShrink;

        public EntryController(ILinkShrink linkShrink)
        {
            _linkShrink = linkShrink;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string longUriStr)
        {
            var longUri = new Uri(longUriStr);
            var shortUri = await _linkShrink.Shrink(longUri);

            return View(new Models.Entry(){LongUri = longUri, ShortUri = shortUri});
        }
    }
}