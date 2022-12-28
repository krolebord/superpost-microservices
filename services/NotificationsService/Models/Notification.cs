namespace NotificationsService.Models;

public class Notification
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public required string Message { get; set; } = string.Empty;
    
    public NotificationContext? Context { get; set; }
}
