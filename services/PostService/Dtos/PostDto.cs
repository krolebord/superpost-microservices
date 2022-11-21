using PostService.Models;

namespace PostService.Dtos;

public record PostDto(Guid Id, string Title, string Content)
{
    public static PostDto FromPost(Post post) => new(post.Id, post.Title, post.Content);
}
