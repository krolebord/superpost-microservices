namespace TimelineService.Models;

public class UserDto
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
}

public class UserPostDto
{
    public required Guid Id { get; init; }
    
    public required string Content { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required int SubPostsCount { get; init; }
    
    public required UserDto User { get; init; }
}

public class FullUserPostDto
{
    public required Guid Id { get; init; }
    
    public required string Content { get; init; }
    
    public required DateTime CreatedAt { get; init; }
    
    public required UserPostDto? ParentPost { get; init; }
    
    public required IEnumerable<UserPostDto> SubPosts { get; init; }
    
    public required UserDto User { get; init; }
}
