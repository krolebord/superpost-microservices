using Common.Messaging.Interfaces;
using Common.Messaging.Options;
using NotifierService.Models;
using PostService.Services;

namespace PostService.Endpoints;

public static class AddEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/halt", (HaltService haltService) => haltService.IsHalted);
        app.MapPost("/halt", (HaltService haltService) => {
            haltService.IsHalted = !haltService.IsHalted;
            return Results.Ok();
        });
        
        app.MapGet("/{id:guid}", PostEndpoints.GetPostById);

        app.MapGet("/last", PostEndpoints.GetLastPosts);
        
        app.MapPost("/", PostEndpoints.CreateUserPost).RequireAuthorization();

        return app;
    }
}
