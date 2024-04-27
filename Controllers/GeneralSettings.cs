using Microsoft.AspNetCore.Mvc;

namespace ThreeFriends.Controllers
{
    public class GeneralSettings : Controller
    {
        // i can send class as binding butt the class is static ):
        public IActionResult Index(DateTime DataRangeFrom ,DateTime DateRangeTo , string Currency , DateTime DeleteOldTo)
        {
            return View();
        }
    }
}
