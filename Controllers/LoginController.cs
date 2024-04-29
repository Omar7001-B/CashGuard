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
            Nuser.photoPath = Nuser.GetPhotoPath(filePath);
            Nuser.Sign_Up_Date = DateTime.Now;
            entity.Users.Add(Nuser);
            entity.SaveChanges();
            return RedirectToAction("index", "Login");
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
