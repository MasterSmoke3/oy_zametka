using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZametkiApp.Data;
using ZametkiApp.Models;

namespace ZametkiApp.Pages
{
    [Authorize]
    public class ExpiredModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ExpiredModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Note> ExpiredNotes { get; set; }

        public async Task OnGetAsync()
        {
            ExpiredNotes = await _context.Notes
                .Where(n => n.Deadline.HasValue && n.Deadline < DateTime.Now)
                .OrderByDescending(n => n.Deadline)
                .ToListAsync();
        }
    }
}
