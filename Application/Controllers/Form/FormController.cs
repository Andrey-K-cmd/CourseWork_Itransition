using Application.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Forms = Application.Models.Form;
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
            return View(new Forms.Form());
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm(Forms.Form form)
        {
            if (ModelState.IsValid)
            {
                _context.Forms.Add(form);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewForm", new {id = form.Id});
            }

            return View(form);
        }

        public IActionResult AddQuestion(int formId)
        {
            var question = new Forms.Question { FormId = formId };
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(Forms.Question question)
        {
            if (!ModelState.IsValid)
            {
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = question.FormId});
            }

            return View(question);
        }

        public async Task<IActionResult> Details(int id)
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
    }
}
