namespace NotifierService.Models;

public class NotificationEto
{
    public required Guid UserId { get; set; }

    public required NotificationDto Notification { get; set; }
}
