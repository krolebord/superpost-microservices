var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/posts");
app.UseRouting();

app.MapHealthChecks("/health");

app.MapGet("/", () => "Hello from PostService!");

app.Run();
