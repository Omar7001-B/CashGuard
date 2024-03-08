using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
	public class RegistrationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SignUp(User user)
		{
			// Save the user to the database
			DB db = new DB();
			db.Add(user);
			db.SaveChanges();
			return Content("User registered");
		}



	}
}
