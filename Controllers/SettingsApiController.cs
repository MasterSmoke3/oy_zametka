using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZametkiApp.Data;
using System.Threading.Tasks;
using ZametkiApp.Models;

namespace ZametkiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SettingsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 👉 API: GET /api/settings
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = "default"; // Заменишь на User.Identity.Name после добавления логина

            var settings = await _context.Settings.FirstOrDefaultAsync(s => s.UserId == userId);

            if (settings == null)
            {
                return Ok(new
                {
                    NotificationsEnabled = true,
                    SoundEnabled = false,
                    SnoozeEnabled = true
                });
            }

            return Ok(new
            {
                settings.EnableNotifications,
                settings.EnableSound,
                settings.EnableSnooze
            });
        }
    }
}
