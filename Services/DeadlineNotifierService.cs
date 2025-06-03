using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZametkiApp.Data;

namespace ZametkiApp.Services
{
    public class DeadlineNotifierService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DeadlineNotifierService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var now = DateTime.UtcNow;
                var notes = db.Notes
                    .Where(n => !n.IsNotified && n.Deadline <= now)
                    .ToList();

                foreach (var note in notes)
                {
                    Console.WriteLine($"🔔 Напоминание о заметке: {note.Title}");
                    note.IsNotified = true;
                }

                await db.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
