namespace NotificationsService.Endpoints;

public static class AddEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/", NotificationsEndpoints.GetNotifications).RequireAuthorization();
        
        app.MapPost("/mark-read", NotificationsEndpoints.MarkNotificationsRead).RequireAuthorization();

        return app;
    }
}
