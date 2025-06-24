using System.Diagnostics;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class HomeController : CustomController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<UserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            var result = _userManager.Users.ToList();
            return View(result);
        }
    }
}
