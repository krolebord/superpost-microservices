namespace UserService.Models;

public class ProfileDto
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required int SubscribersCount { get; init; }
    
    public required int SubscribedToCount { get; init; }
}
