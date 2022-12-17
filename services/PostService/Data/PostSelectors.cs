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
}
