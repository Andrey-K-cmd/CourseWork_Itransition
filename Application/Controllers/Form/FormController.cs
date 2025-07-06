using Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Forms = Application.Models.Form;
using System.Security.Claims;
using Application.Controllers.Custom;
using Application.Models.Answer;
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
            return View(new Forms.FormModel());
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm(Forms.FormModel form)
        {
            form.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (string.IsNullOrEmpty(form.UserId))
            {
                return Unauthorized();
            }

            ModelState.Remove(nameof(form.UserId));

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
                Include(z => z.User).
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
            var form = await _context.Forms
                .FromSqlRaw("SELECT * FROM \"Forms\" WHERE \"TitleVector\" @@ plainto_tsquery({0})", title)
                .FirstOrDefaultAsync();


            if (form == null)
                return RedirectToAction("Home", "Home");

            return RedirectToAction("CompiledForm", new { id = form.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SaveResultForm(FormResponseViewModel model)
        {
            var form = await _context.Forms
                .Include(x => x.Questions)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.FormId);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (form == null || userId == null)
            {
                return NotFound();
            }

            foreach (var asn in model.Answers)
            {
                var answer = new Answer
                {
                    FormId = form.Id,
                    UserId = userId,
                    QuestionId = asn.QuestionId,
                    AnswerText = asn.Answer
                };

                _context.Answers.Add(answer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Home", "Home");
        }

        public async Task<IActionResult> ViewFormResult(int formId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var myForms = await _context.Forms
                .Where(f => f.UserId == userId)
                .ToListAsync();

            var formIds = myForms.Select(f => f.Id).ToList();

            var answers = await _context.Answers
                .Include(a => a.Question)
                .Include(a => a.Form)
                .Include(a => a.User)
                .Where(a => formIds.Contains(a.FormId))
                .OrderBy(a => a.FormId)
                .ThenBy(a => a.UserId)
                .ToListAsync();

            return View("ViewFormResult", answers);
        }
    }
}
