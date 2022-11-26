using AuthService.Data;
using AuthService.Extensions;
using AuthService.Services;
using Common.Auth;
using Common.Clients.UserService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<AuthContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CredentialsService>();
builder.Services.AddTransient<JwtService>();
builder.Services.AddUserServiceClient(builder.Configuration);
builder.Services.AddAuth(builder.Configuration.GetJwtOptions());

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/auth");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();
