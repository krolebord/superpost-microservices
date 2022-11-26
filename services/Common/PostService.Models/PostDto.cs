namespace PostService.Models;

public class PostDto
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required string Content { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required int SubPostsCount { get; init; }
}
