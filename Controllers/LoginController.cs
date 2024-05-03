using Microsoft.AspNetCore.Mvc;
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
        // submit button 
        
    
		// submit button 

	   	[HttpPost] 
        public IActionResult check_sign(string UserName, string Password)
        {
           
            if (UserName == null || string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
            {
                return View("Index");
            }
            
            SharedValues.CurUser.SetCurUser(UserName, Password);
            if (SharedValues.CurUser.User_Name == null)
            {
                return Content("User Not Found");
            }
            else
            {
                return RedirectToAction("Index", "Transaction");
            }
        }

        // submit button , store data in the database 
    
        [HttpPost]
public async Task<IActionResult> AddNew(User Nuser, IFormFile file , string Confirm_Password)
{
            ModelState.Remove("file");
            ModelState.Remove("photoPath");
    if(Confirm_Password != Nuser.Password)
    {
                ModelState.AddModelError("Confirm_Password", "Enter confirm password same as password");
                
    }
    
    if (!ModelState.IsValid)
    {
                return View("Index", Nuser); 
    }

    if (Nuser.IsUser(Nuser.User_Name, Nuser.Password))
    {
                return Content("User already exists");
    }

            if (file == null || file.Length == 0)
            {
                Nuser.photoPath = "/uploads/blank-profile-picture-973460_1280.png";

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
    Nuser.Sign_Up_Date = DateTime.Now;
    entity.Users.Add(Nuser);
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

        // open empty form 
        public IActionResult Index()
        {
            if (SharedValues.CurUser.User_Name != null)
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }
       
    }
}
