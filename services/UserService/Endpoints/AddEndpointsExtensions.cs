namespace UserService.Endpoints;

public static class AddEndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/profile/{idOrName}", ProfileEndpoints.GetProfileById);
        app.MapGet("/profile/me", ProfileEndpoints.GetCurrentUserProfile)
            .RequireAuthorization();

        app.MapGet("/", UserEndpoints.GetUsers);
        app.MapPost("/", UserEndpoints.CreateUser);
        
        app.MapGet("/subscriptions/{userId:guid}", SubscribersEndpoints.GetUserSubscriptions);
        app.MapPost("/subscribe", SubscribersEndpoints.SubscribeCurrentUserTo);
        app.MapPost("/unsubscribe", SubscribersEndpoints.UnsubscribeCurrentUserFrom);
        app.MapGet("/is-subscribed-to/{userId:guid}", SubscribersEndpoints.CheckIfCurrentUserIsSubscribedTo)
            .RequireAuthorization();
        
        return app;
    }
}
