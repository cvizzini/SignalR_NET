using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Hubs
{
    public class MainHub : Hub
    {
        public async IAsyncEnumerable<DateTime> Streaming([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (true)
            {
                yield return DateTime.UtcNow;
                await Task.Delay(60000, cancellationToken);
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
