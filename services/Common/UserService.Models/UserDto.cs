namespace UserService.Models;

public class UserDto
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
}
