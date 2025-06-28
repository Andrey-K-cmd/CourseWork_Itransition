using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : CustomController
    {
        private readonly UserManager<UserModel> _userManager;

        public AdminController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Admin()
        {
            var users = _userManager.Users.ToList();
            var result = new List<(UserModel user, IList<string> Role)>();

            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                result.Add((user, role));
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(List<string> selected)
        {
            foreach (var user in selected)
            {
                var u = await _userManager.FindByEmailAsync(user);

                if (u != null)
                {
                    u.IsBlocked = true;
                    await _userManager.UpdateAsync(u);
                }
            }

            return RedirectToAction("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(List<string> selected)
        {
            foreach (var user in selected)
            {
                var u = await _userManager.FindByEmailAsync(user);

                if (u != null)
                {
                    u.IsBlocked = false;
                    await _userManager.UpdateAsync(u);
                }
            }

            return RedirectToAction("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(List<string> selected)
        {
            foreach (var user in selected)
            {
                var u = await _userManager.FindByEmailAsync(user);

                if (u != null)
                {
                    await _userManager.RemoveFromRoleAsync(u, "User");
                    await _userManager.AddToRoleAsync(u, "Admin");
                }
            }

            return RedirectToAction("Admin");
        }

        public async Task<IActionResult> RemoveAdmin(List<string> selected)
        {
            foreach (var user in selected)
            {
                var u = await _userManager.FindByEmailAsync(user);

                if (u != null)
                {
                    await _userManager.RemoveFromRoleAsync(u, "Admin");
                    await _userManager.AddToRoleAsync(u, "User");
                }
            }

            return RedirectToAction("Admin");
        }
    }
}
