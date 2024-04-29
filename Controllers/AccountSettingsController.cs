using Microsoft.AspNetCore.Mvc;

namespace ThreeFriends.Controllers
{
    public class AccountSettingsController : Controller
    {
        public IActionResult EditInfo()
        {
            return View();
        }
    }
}
