namespace UserService.Models;

public class User
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public ICollection<User> Subscribers { get; } = new HashSet<User>();
    
    public ICollection<User> SubscribedTo { get; } = new HashSet<User>();

}
