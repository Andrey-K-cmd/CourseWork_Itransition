using Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class AccountController : CustomController
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;

        public AccountController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && user.IsBlocked)
                {
                    ModelState.AddModelError("", "Your account is blocked");
                    return View();
                }

                var output = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (output.Succeeded)
                {
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Something wrong with you!");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new UserModel
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                    IsBlocked = false
                };

                var output = await _userManager.CreateAsync(user, model.Password);

                if (output.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    foreach (var er in output.Errors)
                    {
                        ModelState.AddModelError("", er.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Home");
        }
    }
}
