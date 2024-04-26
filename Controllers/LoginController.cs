using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LoginController : Controller
    {
        Appdbcontxt entity = new Appdbcontxt();

        // submit button 
        [HttpPost] // query string
        public IActionResult check_sign(string UserName, string Password)
        {
            if (UserName == null || string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
            {
                return View("Index");
            }
            //var user = entity.Users.FirstOrDefault(u => u.User_Name == UserName && u.Password == Password);
            SharedValues.CurUser.SetCurUser(UserName, Password);
            if (SharedValues.CurUser == null)
            {
                return Content("User Not Found");
            }
            else
            {
                return View("MainPage", SharedValues.CurUser);
            }
        }

        // submit button , store data in the database 
        [HttpPost]
        public IActionResult AddNew(User Nuser)
        { 
            if(Nuser.IsUser(Nuser.User_Name,Nuser.Password))
            {
                return Content("User Already Exists");
            }
            entity.Users.Add(Nuser);
            entity.SaveChanges();
            return View("Index");
            
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

    }
}
