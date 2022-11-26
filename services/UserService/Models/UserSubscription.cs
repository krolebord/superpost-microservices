namespace UserService.Models;

public class UserSubscription
{
    public required Guid UserId { get; set; }
    public User? User { get; set; }
    
    public required Guid SubscribedToId { get; set; }
    public User? SubscribedTo { get; set; }
}
