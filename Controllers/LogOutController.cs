using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult index()
        {
            SharedValues.SetAllNull();
            return RedirectToAction("Index", "Login");
        }
    }
}
