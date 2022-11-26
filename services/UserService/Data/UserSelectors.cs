using System.Linq.Expressions;
using UserService.Models;

namespace UserService.Data;

public static class UserSelectors
{
    public static Expression<Func<User, UserDto>> UserDto { get; } = user => new()
    {
        Id = user.Id,
        Name = user.Name
    };
    
    public static Expression<Func<User, ProfileDto>> ProfileDto { get; } = user => new()
    {
        Id = user.Id,
        Name = user.Name,
        SubscribersCount = user.Subscribers.Count(),
        SubscribedToCount = user.SubscribedTo.Count(),
    };
}
