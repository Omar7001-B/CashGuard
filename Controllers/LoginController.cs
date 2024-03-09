using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LoginController : Controller
    {
        Appdbcontxt entity = new Appdbcontxt();

       // [HttpPost]

        public IActionResult Index(string UserName, string Password)
        {
            if(UserName == null)
            {
                return View();
            }
            var user = entity.Users.FirstOrDefault(u => u.User_Name == UserName && u.Password == Password);
            if (user == null)
            {
                return Content("User Not Found");
            }
            else
            {
                return View("MainPage", user);
            }
            
        }

        public IActionResult Signup()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult AddNew(User Nuser)
        {
            entity.Users.Add(Nuser);
            entity.SaveChanges();
            return View("Index");
        }
    }
}
