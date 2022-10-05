// See https://aka.ms/new-console-template for more information
using SignalR.Server.Hubs;
using SignalR.Server.MinimalApi;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddScoped<SignalRApi>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
      c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
});

builder.Services.AddLogging(opts =>
{
    opts.AddConsole();
    opts.SetMinimumLevel(LogLevel.Information);

});

var app = builder.Build();
app.MapHub<TimeHub>("/current-time");
app.MapHub<MessageHub>("/messages");

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetService<SignalRApi>();
service!.RegisterTodoApi(app);

app.UseSwagger();
app.UseSwaggerUI();
app.MapFallback(() => Results.Redirect("/swagger"));

await app.RunAsync();