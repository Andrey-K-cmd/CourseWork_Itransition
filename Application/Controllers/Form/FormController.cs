using Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Forms = Application.Models.Form;
using System.Security.Claims;
using Application.Controllers.Custom;
namespace Application.Controllers.Form
{
    public class FormController : CustomController
    {
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult NewForm()
        {
            return View(new Forms.FormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm(Forms.FormViewModel form)
        {
            form.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Forms.Add(form);
                await _context.SaveChangesAsync();
                return RedirectToAction("CompiledForm", new {id = form.Id});
            }

            return View("NewForm", form);
        }

        public async Task<IActionResult> CompiledForm(int id)
        {
            var form = await _context.Forms.
                Include(x => x.Questions).
                ThenInclude(y => y.Options).
                FirstOrDefaultAsync(z => z.Id == id);

            if (form == null)
            {
                return NotFound();
            }

            return View(form);

        }

        [HttpGet]
        public async Task<IActionResult> FindForm(string title)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(title) || userId == null)
                return RedirectToAction("Home", "Home");

            var form = await _context.Forms
                .FirstOrDefaultAsync(f => f.Title.ToLower() == title.ToLower());

            if (form == null)
            {
                TempData["NotFound"] = "Form not found.";
                return RedirectToAction("Home", "Home");
            }

            return RedirectToAction("CompiledForm", new { id = form.Id });
        }
    }
}
