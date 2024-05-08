using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class EnsureUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Check(string password)
        {

            User ChkUser = new User();
            bool isuser = ChkUser.IsUser(SharedValues.CurUser.User_Name,password);
            if (!isuser)
            {
                return RedirectToAction("index","LogOut");
            }
            else
            {
                // "/Edit/accountSettingPage"
                return RedirectToAction("accountSettingPage", "Edit");
            }
        }
    }
}
