using Microsoft.AspNetCore.Mvc;

namespace AutoServiceManager.Controllers
{
    public class PlaceholderController : Controller
    {
        [Route("{feature:regex(^(Vehicles|Orders|Parts|Invoices|Reports|Settings)$)}")]
        public IActionResult Index(string feature)
        {
            ViewData["Title"] = feature;
            ViewData["Feature"] = feature;
            return View("~/Views/Shared/ComingSoon.cshtml");
        }
    }
}
