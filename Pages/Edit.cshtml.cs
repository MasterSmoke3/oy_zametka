using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ZametkiApp.Data;
using ZametkiApp.Models;

namespace ZametkiApp.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Note Note { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Note = await _context.Notes.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (Note == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var noteToUpdate = await _context.Notes.FirstOrDefaultAsync(n => n.Id == Note.Id && n.UserId == userId);

            if (noteToUpdate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            noteToUpdate.Title = Note.Title;
            noteToUpdate.Description = Note.Description;
            noteToUpdate.ReminderText = Note.ReminderText;
            noteToUpdate.Deadline = Note.Deadline;
            noteToUpdate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
