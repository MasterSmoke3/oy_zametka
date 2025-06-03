using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZametkiApp.Data;
using ZametkiApp.Models;

namespace ZametkiApp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Note> Notes { get; set; } = new List<Note>();

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                Notes = new List<Note>();
                return;
            }

            var notesQuery = _context.Notes
                .Where(n => n.UserId == userId);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var lowerSearchTerm = SearchTerm.ToLower(); // Заменили ToLowerInvariant() на ToLower()

                notesQuery = notesQuery.Where(n =>
                    (!string.IsNullOrEmpty(n.Title) && n.Title.ToLower().Contains(lowerSearchTerm)) ||
                    (!string.IsNullOrEmpty(n.Description) && n.Description.ToLower().Contains(lowerSearchTerm))
                );
            }

            var notes = await notesQuery.ToListAsync();

            Notes = SortOrder switch
            {
                "newest" => notes
                    .OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.CreatedAt)
                    .ToList(),

                "oldest" => notes
                    .OrderByDescending(n => n.IsPinned)
                    .ThenBy(n => n.CreatedAt)
                    .ToList(),

                _ => notes
                    .OrderByDescending(n => n.IsPinned)
                    .ThenByDescending(n => n.Id)
                    .ToList()
            };
        }

        public async Task<IActionResult> OnPostTogglePinAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (note.UserId != userId)
                {
                    return Forbid();
                }

                note.IsPinned = !note.IsPinned;
                note.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
