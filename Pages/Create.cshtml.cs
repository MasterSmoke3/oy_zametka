using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZametkiApp.Models;
using ZametkiApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ZametkiApp.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Note Note { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Note.CreatedAt = DateTime.Now;
            Note.UpdatedAt = DateTime.Now;

            // Привязываем заметку к текущему пользователю
            Note.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Notes.Add(Note);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }
}
