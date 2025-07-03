using System.Diagnostics;
using Application.Data;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Controllers.Custom;

namespace Application.Controllers.Home
{
    public class HomeController : CustomController
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var form = _context.Forms.Where(x => x.UserId == id).ToList();
            return View(form);
        }
    }
}
