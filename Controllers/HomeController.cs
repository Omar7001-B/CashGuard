using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Appdbcontxt entity = new Appdbcontxt();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
			entity.Database.EnsureCreated();
		}

        public IActionResult Test() // Test Icons
        {
            // Define the icons folder path
            string iconsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icons");

            List<string> iconNames = new List<string>();

            // Get all files in the icons folder
            string[] iconFiles = Directory.GetFiles(iconsFolderPath);

            // Extract file names
            foreach (var iconFile in iconFiles)
            {
                iconNames.Add(Path.GetFileName(iconFile));
            }
            return View(iconNames);
        }

        public IActionResult Index()
        {
            SharedValues.CurUser = new User();
            if(SharedValues.CurUser.User_Name != null)
            {
                SharedValues.CurUser = new User();
                return RedirectToAction("index", "Login");
            }
            return RedirectToAction("index", "Login");
        }


        public ActionResult History()
        {
            var historyItems = entity.History.ToList(); // Fetch all history items from the database
            return View(historyItems);
        }

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
