using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Dtos;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<UsersContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/users");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.MapGet("/{id:guid}", async ([FromRoute] Guid id, UsersContext context) => {
    var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    if (user is null) return Results.NotFound();
    return Results.Ok(new
    {
        user.Id,
        user.Name
    });
});

app.MapPost("/", async ([FromBody] UserCreateDto userDto, UsersContext context) => {
    var user = userDto.ToUser();
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(user.Id);
});

app.Run();
