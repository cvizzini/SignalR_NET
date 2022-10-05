using System.Net.Sockets;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace SignalR.Client.Clients;

public class StreamClient : BackgroundService
{
    private readonly ILogger<StreamClient> _logger;
    private readonly AsyncRetryPolicy _policy;

    public StreamClient(ILogger<StreamClient> logger)
    {
        _logger = logger;
        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync((retryAttempt, context)
                =>
            {
                _logger.LogError($"Error Connecting Retry {retryAttempt}");
                return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
            }
        );

    }
    private void InvokeDate(DateTime date)
    {
        _logger.LogInformation(date.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const string uri = "https://localhost:61345/current-time";

        _logger.LogInformation($"Starting connection : {uri}");

        await using var connection = new HubConnectionBuilder().WithUrl(uri).Build();

        var stream = connection.StreamAsync<DateTime>("Streaming",
              stoppingToken);

        await _policy.ExecuteAsync(connection.StartAsync, stoppingToken);

        await foreach (var date in stream) InvokeDate(date);
    }
}