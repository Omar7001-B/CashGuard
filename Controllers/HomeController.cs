﻿using Microsoft.AspNetCore.Mvc;
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
            SharedValues.CurUser = new User();
            if(SharedValues.CurUser.User_Name != null)
            {
                SharedValues.CurUser = new User();
                return RedirectToAction("index", "Login");
            }
            return RedirectToAction("index", "Login");
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
