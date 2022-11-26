namespace PostService.Endpoints;

public static class AddEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/{id:guid}", PostEndpoints.GetPostById);

        app.MapGet("/last", PostEndpoints.GetLastPosts);
        
        app.MapPost("/", PostEndpoints.CreateUserPost).RequireAuthorization();

        return app;
    }
}
