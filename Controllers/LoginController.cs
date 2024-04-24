using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LoginController : Controller
    {
        Appdbcontxt entity = new Appdbcontxt();

        // submit button 
        [HttpPost]
        public IActionResult check_sign(string UserName, string Password)
        {
            if (UserName == null || string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
            {
                return View("Index");
            }
            //var user = entity.Users.FirstOrDefault(u => u.User_Name == UserName && u.Password == Password);
            SharedValues.SetCurUser(UserName, Password);
            if (SharedValues.CurUser == null)
            {
                return Content("User Not Found");
            }
            else
            {
                return View("MainPage", SharedValues.CurUser);
            }
        }

        // open empty form 
        public IActionResult Index()
        { 
            return View();
        }

        // open empty form 
        public IActionResult Signup()
        {
            return View();  
        }

        // submit button , store data in the database 
        [HttpPost]
        public IActionResult AddNew(User Nuser)
        {
            // validation function needed in class user
            // tempdata["user"] = nuser ; 
            // user.ini
            string UserName = Nuser.User_Name;
            string Password = Nuser.Password;
            // 
            if(SharedValues.IsUser(UserName, Password))
            {
                RedirectToAction("signup");
                return Content("User Already Exists");
            }
            entity.Users.Add(Nuser);
            entity.SaveChanges();
            return View("Index");
        }
    }
}
