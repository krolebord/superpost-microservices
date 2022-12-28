using Common.Auth;
using Common.Clients.UserService;
using Common.Messaging;
using Common.Messaging.Options;
using Microsoft.EntityFrameworkCore;
using NotificationsService.Data;
using NotificationsService.Endpoints;
using NotificationsService.Handlers;
using PostService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<NotificationsContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration.GetJwtOptions());
builder.Services.AddUserServiceClient(builder.Configuration);
builder.Services.AddMessaging(new()
{
    ConnectionString = builder.Configuration.GetConnectionString("RabbitMQ") ?? throw new Exception("Unspecified RabbitMQ Connection string")
});
builder.Services.AddMessageHandler<PostCreatedEto, PostCreatedHandler>(new()
{
    ExchangeName = "event.exchange",
    ExchangeType = ExchangeType.Topic,
    QueueName = "event.post.create.to.notifications",
    RoutingKey = "event.post.create.#",
    SequentialFetch = true
});

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/notifications");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();
