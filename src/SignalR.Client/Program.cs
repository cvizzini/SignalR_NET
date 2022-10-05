// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalR.Client.Clients;

Console.WriteLine("Hello, World!");

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<StreamClient>();
        services.AddHostedService<MessageClient>();
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.ClearProviders();                        
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();