namespace PostService.Models;

public record FullPostDto
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required string Content { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required PostDto? ParentPost { get; init; }

    public required IEnumerable<PostDto> SubPosts { get; init; }
}
