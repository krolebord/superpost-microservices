using Common.Auth;
using Common.Messaging;
using Microsoft.EntityFrameworkCore;
using PostService;
using PostService.Data;
using PostService.Endpoints;
using PostService.Middlewares;
using PostService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<PostsContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<HaltService>();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration.GetJwtOptions());
builder.Services.AddMessaging(new()
{
    ConnectionString = builder.Configuration.GetConnectionString("RabbitMQ") ?? throw new Exception("Unspecified RabbitMQ Connection string")
});

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/posts");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SimulatedLatencyMiddleware>();

app.MapEndpoints();

app.Run();
