using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddDbContext<PostsContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/posts");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.MapGet("/{id:guid}", async ([FromRoute] Guid id, PostsContext context) => {
    var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == id);
    if (post is null) return Results.NotFound();
    return Results.Ok(new
    {
        post.Id,
        post.Title,
        post.Content
    });
});

app.MapPost("/", async ([FromBody] PostCreateDto postDto, PostsContext context) => {
    var user = postDto.ToPost();
    context.Posts.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(user.Id);
});

app.Run();
