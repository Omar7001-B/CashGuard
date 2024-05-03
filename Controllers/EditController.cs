using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;
using System.Text.RegularExpressions;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.AspNetCore.Hosting;

namespace ThreeFriends.Controllers
{
    public class EditController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        Appdbcontxt db = new Appdbcontxt();
        private User? CurUser;
      

        public EditController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        /*  public IActionResult Index()
          {

              List<User> userList = db.Users.ToList();
              return View(userList);
          }*/
        [HttpPost]
        /*public IActionResult testLogin(User testUser)
        {
            List<User> users = db.Users.ToList();
            foreach (User user in users)
            {
                if (user.UserName == testUser.UserName && user.Password == testUser.Password)
                {
                    currentUser = user;

                    return RedirectToAction("WelcomePage");
                }
               
            }
            return View("Login" , testUser);
        }*/
        public IActionResult Login()
        {
            return View(new User());

        }
       
        public IActionResult AccountSettingPage()
        {
            CurUser = db.Users.FirstOrDefault(U => U.User_Name == HttpContext.Session.GetString("UserName")
                                                                   && U.Password == HttpContext.Session.GetString("Password"));
            
            return View(CurUser);
        }

        public IActionResult emailSetting()
        {
            return View(new User());
        }
        public IActionResult saveEmailSetting(User test)
        {
            Regex r = new Regex(@"(^[a-zA-z]+[0-9]*@[a-z]+\.[a-z]{3}$)");
            bool testE = !string.IsNullOrEmpty(test.Email) && r.Match(test.Email).Success;
            if (testE)
            {
                CurUser.Email = test.Email;
                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurUser.Id);
                userToUpdate.Email = CurUser.Email;
                db.SaveChanges();
                return RedirectToAction("AccountSettingPage");
            }
            else
                return View("emailSetting", test);
        }
        public IActionResult passwordSetting() { return View(new User()); }
        public IActionResult savePasswordSetting(User test, string confirmPass)
        {

            Regex r = new Regex(@"(^(?=.*[A-Z])(?=.*[\d])(?=.*[\W_]).{8,}$)");
            bool testP = !string.IsNullOrEmpty(test.Password) && r.Match(test.Password).Success;
            bool testCP = !string.IsNullOrEmpty(confirmPass) && r.Match(confirmPass).Success;
            //dd

            if (testP && testCP && (test.Password == confirmPass))
            {
                CurUser.Password = test.Password;

                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurUser.Id);
                userToUpdate.Password = CurUser.Password;
                db.SaveChanges();
                return RedirectToAction("AccountSettingPage");
            }
            else return View("passwordSetting", test);
        }
        public IActionResult nameSetting() { return View(new User()); }
        public IActionResult saveNameSetting(User test)
        {
            CurUser = db.Users.FirstOrDefault(U => U.User_Name == HttpContext.Session.GetString("UserName")
                                                                  && U.Password == HttpContext.Session.GetString("Password"));
            Regex r = new Regex(@"(^[a-zA-Z][a-zA-Z][a-zA-Z]*$)");

            bool testfn = !string.IsNullOrEmpty(test.First_Name) && r.Match(test.First_Name).Success;
            bool testln = !string.IsNullOrEmpty(test.Last_Name) && r.Match(test.Last_Name).Success;
            if (string.IsNullOrWhiteSpace(test.First_Name) && string.IsNullOrWhiteSpace(test.Last_Name))
            {
                return View("nameSetting", test);
            }
            if (test.First_Name != null && string.IsNullOrWhiteSpace(test.Last_Name))
            {
                if (testfn == true)
                {
                    CurUser.First_Name = test.First_Name;
                    db.SaveChanges();
                    return RedirectToAction("AccountSettingPage");

                }
                else
                {
                    return View("nameSetting", test);
                }

            }
            if (test.Last_Name != null && string.IsNullOrWhiteSpace(test.First_Name))
            {
                if (testln == true)
                {
                    CurUser.Last_Name = test.Last_Name;
                    User userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurUser.Id);
                    userToUpdate.Last_Name = CurUser.Last_Name;
                    db.SaveChanges();
                    return RedirectToAction("AccountSettingPage");

                }
                else
                {
                    return View("nameSetting", test);
                }
            }
            if (test.Last_Name != null && test.First_Name != null)
            {
                if ((testfn == true) && (testln == true))
                {
                    CurUser.Last_Name = test.Last_Name;
                    CurUser.First_Name = test.First_Name;
                    User userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurUser.Id);
                    userToUpdate.First_Name = CurUser.First_Name;
                    userToUpdate.Last_Name = CurUser.Last_Name;
                    db.SaveChanges();
                    return RedirectToAction("AccountSettingPage");


                }
                else
                {
                    return View("nameSetting", test);
                }

            }
            else
                return View("nameSetting", test);
        }

        [HttpPost]
        public IActionResult deleteAccount()
        {
            CurUser = db.Users.FirstOrDefault(U => U.User_Name == HttpContext.Session.GetString("UserName")
                                                                   && U.Password == HttpContext.Session.GetString("Password"));
            db.Users.Remove(CurUser);
            db.SaveChanges();
            CurUser = new User();
            HttpContext.Session.Clear();
            return RedirectToAction("index", "LogOut");
        }

        public IActionResult imageSetting()
        {
            return View(new User());
        }
        public IActionResult saveImageSetting(User test, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!ImageFile.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("ImageFile", "Please upload a valid image file.");
                    return View("imageSetting", test);
                }

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                // Check if the directory exists, create it if it doesn't
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                string imagePath = "/images/" + uniqueFileName;
                CurUser.photoPath = imagePath;
                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurUser.Id);
                userToUpdate.photoPath = CurUser.photoPath;
                db.SaveChanges();
                return RedirectToAction("AccountSettingPage");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please upload an image.");
                return View("ImageSetting", test);
            }
        }

    }
}
