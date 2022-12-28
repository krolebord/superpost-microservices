using System.Security.Claims;
using Common.Auth;
using Microsoft.EntityFrameworkCore;
using NotificationsService.Data;

namespace NotificationsService.Endpoints;

public static class NotificationsEndpoints
{
    public static async Task<IResult> GetNotifications(
        ClaimsPrincipal user,
        NotificationsContext context)
    {
        var userId = user.GetUserId();

        var notifications = await context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(NotificationSelectors.NotificationDto)
            .ToListAsync();

        return Results.Ok(notifications);
    }
    
    public static async Task<IResult> MarkNotificationsRead(
        ClaimsPrincipal user,
        NotificationsContext context)
    {
        var userId = user.GetUserId();

        var notifications = await context.Notifications
                .Where(x => x.UserId == userId)
                .ToListAsync();

        var now = DateTime.UtcNow;
        foreach (var notification in notifications)
        {
            notification.ReadAt = now;
        }

        await context.SaveChangesAsync();
        
        return Results.Ok();
    }
}
