using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Schedent.API.Hubs
{
    public class NotificationHub : Hub
    {
        public Task SendNotification(int user, string message)
        {
            return Clients.All.SendAsync("ReceiveOne", message);
        }
    }
}