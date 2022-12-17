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
        var posts = await context.PostDtos
            .FromSql($"""
                WITH RECURSIVE post AS (
                    SELECT *
                    FROM "Posts"
                    WHERE "Id" = {id} or "ParentPostId" = {id}
                    
                    UNION DISTINCT
                    SELECT parent.*
                    FROM "Posts" parent
                        INNER JOIN post o ON o."ParentPostId" = parent."Id"
                )
                SELECT *, (SELECT count("Id") from "Posts" where "ParentPostId" = post."Id") as "SubPostsCount" FROM post
                ORDER BY "CreatedAt";
            """)
            .AsNoTracking()
            .ToListAsync();

        if (!posts.Any())
        {
            return Results.NotFound();
        }

        List<PostDto> parentPosts = new();
        PostDto? targetPost = null;
        List<PostDto> subPosts = new();

        foreach (var post in posts)
        {
            if (post.Id == id)
            {
                targetPost = post;
            }
            else if (targetPost is null)
            {
                parentPosts.Add(post);
            }
            else
            {
                subPosts.Add(post);
            }
        }

        if (targetPost is null)
        {
            throw new Exception();
        }

        FullPostDto dto = new()
        {
            Id = id,
            UserId = targetPost.UserId,
            Content = targetPost.Content,
            CreatedAt = targetPost.CreatedAt,
            ParentPosts = parentPosts,
            SubPosts = subPosts
        };
        
        return Results.Ok(dto);
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
