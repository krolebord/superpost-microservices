using System.Security.Claims;
using Common.Auth;
using Common.Clients.PostService;
using Common.Clients.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimelineService;
using TimelineService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuth(builder.Configuration.GetJwtOptions());
builder.Services.AddPostServiceClient(builder.Configuration);
builder.Services.AddUserServiceClient(builder.Configuration);

var app = builder.Build();

app.UsePathBase(app.Configuration["ASPNETCORE_BASE"] ?? "/api/timeline");
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/home", [Authorize] async (
    ClaimsPrincipal user,
    PostServiceClient postServiceClient,
    UserServiceClient userServiceClient
) => {
    var userId = user.GetUserId();
    var users = (await userServiceClient.GetUserSubscriptions(userId))
        .ToDictionary(x => x.Id);

    if (users.Count == 0)
    {
        return Results.Ok(Array.Empty<object>());
    }

    var posts = await postServiceClient.GetLastPosts(fromUsers: users.Keys);

    var result = posts.Select(post => UserPostMapper.CreateUserPostDto(post, users[post.UserId]));
    return Results.Ok(result);
});

app.MapGet("/new", async (
    PostServiceClient postServiceClient,
    UserServiceClient userServiceClient
) => {
    var posts = await postServiceClient.GetLastPosts();

    var userIds = posts.Select(x => x.UserId).ToArray();
    var users = (await userServiceClient.GetUsers(userIds))
        .ToDictionary(x => x.Id);
    
    var result = posts.Select(post => UserPostMapper.CreateUserPostDto(post, users[post.UserId]));
    return Results.Ok(result);
});

app.MapGet("/new/{userId:guid}", async (
    [FromRoute] Guid userId,
    PostServiceClient postServiceClient,
    UserServiceClient userServiceClient
) => {
    var user = (await userServiceClient.GetUsers(new [] { userId })).FirstOrDefault();
    if (user is null) return Results.NotFound();

    var lastPosts = await postServiceClient.GetLastPosts(fromUser: userId);

    var result = lastPosts.Select(post => UserPostMapper.CreateUserPostDto(post, user));
    
    return Results.Ok(result);
});

app.MapGet("/post/{postId:guid}", async (
    [FromRoute] Guid postId,
    PostServiceClient postServiceClient,
    UserServiceClient userServiceClient
) => {
    var post = await postServiceClient.GetFullPost(postId);
    if (post is null) return Results.NotFound();

    var userIds = post.SubPosts
        .Select(x => x.UserId)
        .Append(post.UserId)
        .Concat(post.ParentPosts.Select(x => x.UserId))
        .ToHashSet();
    
    var users = (await userServiceClient.GetUsers(userIds))
        .ToDictionary(x => x.Id);

    var result = new FullUserPostDto
    {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        User = users[post.UserId].ToUserDto(),
        ParentPosts = post.ParentPosts.Any()
            ? post.ParentPosts.Select(parentPost => UserPostMapper.CreateUserPostDto(parentPost, users[parentPost.UserId]))
            : null,
        SubPosts = post.SubPosts
            .Select(x => UserPostMapper.CreateUserPostDto(x, users[x.UserId]))
    };
    
    return Results.Ok(result);
});

app.Run();
