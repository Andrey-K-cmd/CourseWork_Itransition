using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class AdminController : CustomController
    {
        private readonly UserManager<UserModel> _userManager;

        public AdminController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var result = _userManager.Users.ToList();
            return View(result);
        }
    }
}
