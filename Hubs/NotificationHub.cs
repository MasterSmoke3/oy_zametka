using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ZametkiApp.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendReminder(string message)
        {
            await Clients.All.SendAsync("ReceiveReminder", message);
        }
    }
}
