using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class LoginController : Controller
    {
        Appdbcontxt entity = new Appdbcontxt();
        private readonly IWebHostEnvironment _webHost; 
        public LoginController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        private void SetSessionData(string username , string password)
        {
            User CurUser = entity.Users.FirstOrDefault(U => U.User_Name == username);
            SharedValues.CurUser = CurUser;
            HttpContext.Session.SetString("UserName", CurUser.User_Name);
            HttpContext.Session.SetString("Password", password);
            HttpContext.Session.SetString("FirsName", CurUser.First_Name);
            HttpContext.Session.SetString("LastName", CurUser.Last_Name);
            HttpContext.Session.SetString("PhotoPath", CurUser.photoPath);
            HttpContext.Session.SetString("DateIn", CurUser.Sign_Up_Date.ToString());
            HttpContext.Session.SetString("Email", CurUser.Email);
            HttpContext.Session.SetString("PhoneNumber", CurUser.Phone_Number);
            HttpContext.Session.SetString("Id", CurUser.Id.ToString());
        }
        [HttpPost] 
        public IActionResult check_sign(string UserName, string Password)
        {
            // if user is already logged in redirect to the main page
            if(HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Transaction");
            }
            if (UserName == null || string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
            {
                return View("Index");
            }
            User ChkUser = new User();
            bool is_user = ChkUser.IsUser(UserName, Password);
            if (!is_user)
            {
                return View("Check_sign");
            }
            else
            {
                SetSessionData(UserName, Password);
                return RedirectToAction("Index", "Transaction");
            }

            
        }

        // submit button , store data in the database 
        [HttpPost]
        public async Task<IActionResult> AddNew(User Nuser , IFormFile file)
        {
            ModelState.Remove("photoPath");
            if (!ModelState.IsValid)
            {
                return View("Index",Nuser);
            }
            if(Nuser.IsUser(Nuser.User_Name,Nuser.Password))
            {
                return Content("User Already Exists");
            }
            string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using(FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            Nuser.Password = Hashing.HashPassword(Nuser.Password);
            Nuser.photoPath = Nuser.GetPhotoPath(filePath);
            Nuser.Sign_Up_Date = DateTime.Now;
            entity.Users.Add(Nuser);
            entity.SaveChanges();
            return RedirectToAction("index", "Login");
        }

        // this fuction is zero refernce but it is important and used in the _layout line 19
        public IActionResult IsUserLogin() 
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Transaction");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public IActionResult LogOut()
        { 
            return RedirectToAction("Index", "Login");
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }
       
    }
}
