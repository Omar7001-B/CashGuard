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

        public EditController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        Appdbcontxt db = new Appdbcontxt();
        
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
     /*   public IActionResult WelcomePage() 
        {

            return View(SharedValuecurrentUser);
        }*/
        public IActionResult AccountSettingPage()
        {
            return View(SharedValues.CurUser);
        }
        
         public IActionResult emailSetting()
         {
             return View(new User());
         }
        public IActionResult saveEmailSetting(User test)
        {
            Regex r = new Regex(@"(^[a-zA-z]+[0-9]*@[a-z]+\.[a-z]{3}$)");
            bool testE = !string.IsNullOrEmpty(test.Email) && r.Match(test.Email).Success;
            if(testE)
            {
                SharedValues.CurUser.Email = test.Email;
                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                userToUpdate.Email = SharedValues.CurUser.Email;
                db.SaveChanges();
                return RedirectToAction("AccountSettingPage");
            }
            else
                return View("emailSetting",test);
        }
       public IActionResult passwordSetting() { return View(new User()); }
        public IActionResult savePasswordSetting(User test , string confirmPass)
        {
            
            Regex r = new Regex(@"(^(?=.*[A-Z])(?=.*[\d])(?=.*[\W_]).{8,}$)");
            bool testP = !string.IsNullOrEmpty(test.Password) && r.Match(test.Password).Success;
            bool testCP = !string.IsNullOrEmpty(confirmPass) && r.Match(confirmPass).Success;
            //dd

            if (testP&&testCP&&(test.Password==confirmPass))
            {
                SharedValues.CurUser.Password = test.Password;
                
                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                userToUpdate.Password = SharedValues.CurUser.Password;
                db.SaveChanges();
                return RedirectToAction("AccountSettingPage");
            }
            else return View("passwordSetting", test );
        }
        public IActionResult nameSetting() { return View(new User()); }
        public IActionResult saveNameSetting(User test)
        {
            Regex r = new Regex(@"(^[a-zA-Z][a-zA-Z][a-zA-Z]*$)");

            bool testfn = !string.IsNullOrEmpty(test.First_Name) && r.Match(test.First_Name).Success;
            bool testln = !string.IsNullOrEmpty(test.Last_Name) && r.Match(test.Last_Name).Success;
            if (string.IsNullOrWhiteSpace(test.First_Name) && string.IsNullOrWhiteSpace(test.Last_Name))
            {
                return View ("nameSetting", test);
            }
            if(test.First_Name != null && string.IsNullOrWhiteSpace(test.Last_Name))
            {
                if (testfn==true)
                {
                    SharedValues.CurUser.First_Name = test.First_Name;
                    User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                    userToUpdate.First_Name = SharedValues.CurUser.First_Name;
                    db.SaveChanges();
                    return RedirectToAction("AccountSettingPage");

                }
                else
                {
                    return View("nameSetting", test);
                }
               
            }
            if(test.Last_Name != null && string.IsNullOrWhiteSpace(test.First_Name))
            {
                if (testln == true)
                {
                    SharedValues.CurUser.Last_Name = test.Last_Name;
                    User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                    userToUpdate.Last_Name = SharedValues.CurUser.Last_Name;
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
                if ((testfn==true) && (testln==true))
                {
                    SharedValues.CurUser.Last_Name = test.Last_Name;
                    SharedValues.CurUser.First_Name = test.First_Name;
                    User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                    userToUpdate.First_Name = SharedValues.CurUser.First_Name;
                    userToUpdate.Last_Name = SharedValues.CurUser.Last_Name;
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
            db.Users.Remove(SharedValues.CurUser);
            db.SaveChanges();
            SharedValues.CurUser = new User();
            return RedirectToAction("index","LogOut");
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
                SharedValues.CurUser.photoPath = imagePath;
                User userToUpdate = db.Users.FirstOrDefault(u => u.Id == SharedValues.CurUser.Id);
                userToUpdate.photoPath = SharedValues.CurUser.photoPath;
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
