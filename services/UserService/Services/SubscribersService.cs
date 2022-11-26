using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Services;

public class SubscribersService
{
    private readonly UsersContext _context;

    public SubscribersService(UsersContext context)
    {
        _context = context;
    }
    public async Task SubscribeUserTo(Guid subscriberId, params Guid[] userIds)
    {
        var existingUsersIds = await _context.Users
            .Where(x => userIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        var existingSubscriptionsIds = await _context.Subscriptions
            .Where(x => x.UserId == subscriberId && userIds.Contains(x.SubscribedToId))
            .Select(x => x.SubscribedToId)
            .ToListAsync();
        existingSubscriptionsIds.Add(subscriberId);

        var newSubscriptions = existingUsersIds
            .Except(existingSubscriptionsIds)
            .Select(x => new UserSubscription
            {
                UserId = subscriberId,
                SubscribedToId = x
            });

        _context.Subscriptions.AddRange(newSubscriptions);
        await _context.SaveChangesAsync();
    }

    public async Task UnsubscribeUserFrom(Guid subscriberId, params Guid[] userIds)
    {
        var removedSubscriptions = await _context.Subscriptions
            .Where(x => x.UserId == subscriberId && userIds.Contains(x.SubscribedToId))
            .ToListAsync();
        
        _context.Subscriptions.RemoveRange(removedSubscriptions);
        await _context.SaveChangesAsync();
    }
}
