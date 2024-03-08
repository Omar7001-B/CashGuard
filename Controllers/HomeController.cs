using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

// You can send data from view to controller using Get or Post method.
// Get is sending data through the URL in the query string.
// Fields with attribute name are sent to the controller.
// Example : url?name=John&age=25
// Post is sending data through the body of the request. (Hidden and secure)
// Also you can send data using the anchor tag  --> url?name=John&age=25
// Also you can send using ajax0


//  Model Binding in ASP.NET Core
//  Model binding is the process of mapping the HTTP request data to the action method parameters.
