using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
    options.ListenAnyIP(5000, listenOptions =>
        {
          listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        }));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.MapGet("/hello", () => "Hello from PostService!");

app.Run();
