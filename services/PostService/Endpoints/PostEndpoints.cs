using System.Security.Claims;
using Common.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Models;

namespace PostService.Endpoints;

public static class PostEndpoints
{
    public static async Task<IResult> GetPostById(
        [FromRoute] Guid id,
        PostsContext context)
    {
        var post = await context.Posts
            .Include(x => x.SubPosts)
            .Select(PostSelectors.FullPostDto)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (post is null) return Results.NotFound();
        return Results.Ok(post);
    }

    public static async Task<IResult> GetLastPosts(
        PostsContext context,
        [FromQuery] int limit = 20,
        [FromQuery] bool filterSubPosts = true,
        [FromQuery] Guid[]? fromUser = null)
    {
        IQueryable<Post> lastPostsQuery = context.Posts
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit);

        if (fromUser is not null && fromUser.Length > 0)
        {
            lastPostsQuery = lastPostsQuery.Where(x => fromUser.Contains(x.UserId));
        }
        if (filterSubPosts)
        {
            lastPostsQuery = lastPostsQuery.Where(x => x.ParentPostId == null);
        }
        
        var postDtos = await lastPostsQuery
            .Select(PostSelectors.PostDto)
            .ToListAsync();
        
        return Results.Ok(postDtos);
    }
    
    public static async Task<IResult> CreateUserPost(
        [FromBody] PostCreateDto postDto,
        ClaimsPrincipal user,
        PostsContext context)
    {
        var post = postDto.ToPost(
            Guid.NewGuid(),
            user.GetUserId(),
            DateTime.UtcNow
        );
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return Results.Ok(post.Id);
    }
}
