using NotificationsService.Models;
using NotifierService.Models;

namespace NotificationsService.Data;

public class NotificationMappers
{
    public static Func<Notification, NotificationDto> NotificationDto = NotificationSelectors.NotificationDto.Compile();
}
