namespace NotifierService.Models;

public class NotificationEto
{
    public Guid UserId { get; set; }
    
    public required NotificationDto Notification { get; set; }
}
