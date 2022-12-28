using System.Linq.Expressions;
using NotificationsService.Models;
using NotifierService.Models;

namespace NotificationsService.Data;

public class NotificationSelectors
{
    public static Expression<Func<Notification, NotificationDto>> NotificationDto = notification => new()
    {
        Id = notification.Id,
        SentAt = notification.CreatedAt,
        ReadAt = notification.ReadAt,
        Message = notification.Message,
        ContextId = notification.Context == null ? null : notification.Context.Id,
        ContextType = notification.Context == null ? null : notification.Context.Type
    };
}
