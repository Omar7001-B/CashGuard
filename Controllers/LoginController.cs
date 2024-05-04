using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        private void SetSessionData(string username, string password)
        {
            User CurUser = entity.Users.FirstOrDefault(U => U.User_Name == username);
            string DummyPassword = new string('*', password.Length);
            SharedValues.CurUser = CurUser;
            SharedValues.CurUser.Password = DummyPassword;
            if (HttpContext.Session.GetString("LastName") == null)
            {
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
            else
            {
                return;
            }
        }
        [HttpPost]
        public IActionResult check_sign(string UserName, string Password)
        {
            // if user is already logged in redirect to the main page
            if (HttpContext.Session.GetString("UserName") != null)
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

        public async Task<IActionResult> AddNew(User Nuser, IFormFile file, string Confirm_Password)
        {
            ModelState.Remove("file");

            ModelState.Remove("photoPath");
            if (Confirm_Password != Nuser.Password)
            {
                ModelState.AddModelError("Confirm_Password", "Enter confirm password same as password");

            }
            if (string.IsNullOrEmpty(Nuser.Gender))
            {
                ModelState.AddModelError("Gender", "Please select a gender");
            }
            if (!ModelState.IsValid)
            {
                return View("Index", Nuser);
            }

            // need another view
            if (Nuser.IsUser(Nuser.User_Name, Nuser.Password))
            {
                return Content("User already exists");
            }

            if (file == null || file.Length == 0)
            {
                if (Nuser.Gender == "Male")
                {
                    Nuser.photoPath = "/uploads/MaleDefault.png";
                }
                else
                {
                    Nuser.photoPath = "/uploads/FemalDefault.png";
                }
            }

            else if (file != null && !IsImage(file))

            {
                ModelState.AddModelError("photoPath", "Please upload an image file (jpg, jpeg, png, gif, bmp).");
                return View("Index", Nuser);
            }
            else if (file != null && IsImage(file))
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Nuser.photoPath = Nuser.GetPhotoPath(filePath);
            }
            Nuser.Password = Hashing.HashPassword(Nuser.Password);
            Nuser.Sign_Up_Date = DateTime.Now;
            entity.Users.Add(Nuser);
            entity.SaveChanges();

            HistoryItem historyItem = new HistoryItem
            {
                OperationType = "User Registration",
                Details = $"User '{Nuser.User_Name}' registered.",
                Timestamp = DateTime.Now,
                UserId = Nuser.Id,
            };

            entity.History.Add(historyItem);
            entity.SaveChanges();
            return RedirectToAction("Index", "Login");
        }

        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.ToLower() == "image/jpg" ||
                file.ContentType.ToLower() == "image/jpeg" ||
                file.ContentType.ToLower() == "image/png" ||
                file.ContentType.ToLower() == "image/gif" ||
                file.ContentType.ToLower() == "image/bmp")
            {
                return true;
            }
            return false;
        }




        // this fuction is zero refernce but it is important and used in the _layout line 19
        public IActionResult IsUserLogin()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Transaction");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}
