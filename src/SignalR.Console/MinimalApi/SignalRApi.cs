using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Hubs;
using Swashbuckle.AspNetCore.Annotations;

namespace SignalR.Server.MinimalApi;

public class SignalRApi
{
    private readonly ILogger<SignalRApi> _logger;
    private readonly IHubContext<MessageHub> _hub;
    public SignalRApi(ILogger<SignalRApi> logger, IHubContext<MessageHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public void RegisterTodoApi(WebApplication app)
    {
        app.MapPost("/api/signalR/{id}", async ([FromRoute, Required] int id) =>
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", $"Message from Server {id}");
                return Results.Ok(id);
            })
            .WithName("Post Message")
            .WithTags("SignalR")
            .Produces<int>()
            .WithMetadata(new SwaggerOperationAttribute("Post Message", "Post message with Signal R"));
    }
}