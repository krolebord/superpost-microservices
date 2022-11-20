var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/users");
app.UseRouting();

app.MapHealthChecks("/health");

app.MapGet("/", () => "Hello from UserService!");

app.Run();
