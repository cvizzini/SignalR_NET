using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Server.Hubs
{
    public class TimeHub : Hub
    {
        public async IAsyncEnumerable<DateTime> Streaming([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (true)
            {
                yield return DateTime.UtcNow;
                await Task.Delay(60000, cancellationToken);
            }
        }
    }
}
