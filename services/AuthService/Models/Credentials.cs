namespace AuthService.Models;

public class Credentials
{
    public required Guid Id { get; set; }
    
    public required Guid UserId { get; set; }
    
    public required string UserName { get; set; }
    
    public required string Email { get; set; }
    
    public required string PasswordSalt { get; set; }

    public required string PasswordHash { get; set; }
}
