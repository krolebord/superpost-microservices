using Common.Auth;
using Common.Messaging;
using Common.Messaging.Options;
using Lib.AspNetCore.ServerSentEvents;
using NotifierService;
using NotifierService.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth(builder.Configuration.GetJwtOptions());
builder.Services.AddHealthChecks();
builder.Services.AddCors();
builder.Services.AddServerSentEvents<INotifierService, NotifierService.NotifierService>();
builder.Services.AddMessaging(new()
{
    ConnectionString = builder.Configuration.GetConnectionString("RabbitMQ") ?? throw new Exception("Unspecified RabbitMQ Connection string")
});
builder.Services.AddMessageHandler<NotificationEto, NotificationsHandler>(new()
{
    ExchangeName = "event.exchange",
    ExchangeType = ExchangeType.Topic,
    QueueName = $"event.notification.create.to.notifier.{Guid.NewGuid()}",
    RoutingKey = "event.notification.create.#"
});


var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/notifier");
app.UseRouting();

app.UseCors(opts => opts
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    );

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapServerSentEvents<NotifierService.NotifierService>("/new", new()
{
    Authorization = ServerSentEventsAuthorization.Default,
    OnPrepareAccept = response =>
    {
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("X-Accel-Buffering", "no");
    }
});

app.Run();
