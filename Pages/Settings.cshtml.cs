using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZametkiApp.Data;
using ZametkiApp.Models;
using System.Threading.Tasks;


namespace ZametkiApp.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SettingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserSettings Settings { get; set; } = new();

        private string GetCurrentUserId()
        {
            // Пока у нас нет регистрации — подставим временного "пользователя"
            return "test-user";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();

            Settings = await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (Settings == null)
            {
                Settings = new UserSettings { UserId = userId };
                _context.Settings.Add(Settings);
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = GetCurrentUserId();

            var existingSettings = await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (existingSettings != null)
            {
                existingSettings.EnableNotifications = Settings.EnableNotifications;
                existingSettings.EnableSound = Settings.EnableSound;
                existingSettings.EnableSnooze = Settings.EnableSnooze;
            }
            else
            {
                Settings.UserId = userId;
                _context.Settings.Add(Settings);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(); // Обновим страницу
        }
    }
}
