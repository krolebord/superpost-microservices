using System.Linq.Expressions;
using PostService.Models;

namespace PostService.Data;

public static class PostSelectors
{
    public static Expression<Func<Post, PostDto>> PostDto { get; } = post => new()
    {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        UserId = post.UserId,
        SubPostsCount = post.SubPosts.Count()
    };
    
    public static Expression<Func<Post, FullPostDto>> FullPostDto { get; } = post => new()
    {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        UserId = post.UserId,
        ParentPost = post.ParentPost != null
            ? new PostDto
            {
                Id = post.ParentPost.Id,
                Content = post.ParentPost.Content,
                CreatedAt = post.ParentPost.CreatedAt,
                UserId = post.ParentPost.UserId,
                SubPostsCount = post.ParentPost.SubPosts.Count()
            } : null,
        SubPosts = post.SubPosts.AsQueryable().Select(PostDto).AsEnumerable()
    };
}
