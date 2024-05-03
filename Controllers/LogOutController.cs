using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult index()
        {
            SharedValues.SetAllNull();
            HttpContext.Session.Clear();
            return RedirectToAction("LogOut", "Login");
        }
    }
}
