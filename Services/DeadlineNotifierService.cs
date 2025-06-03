using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZametkiApp.Data;
using ZametkiApp.Hubs;

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
                var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                var now = DateTime.UtcNow;
                var notes = db.Notes
                    .Where(n => !n.IsNotified && n.Deadline <= now)
                    .ToList();

                foreach (var note in notes)
                {
                    await hub.Clients.All.SendAsync("ReceiveReminder", $"🔔 Напоминание: {note.Title}", stoppingToken);
                    note.IsNotified = true;
                }

                await db.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
