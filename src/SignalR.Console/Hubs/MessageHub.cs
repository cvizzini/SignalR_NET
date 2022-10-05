using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
