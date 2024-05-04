using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult index()
        {
            GeneralSettings.SetAllNull();
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Login");
        }
    }
}
