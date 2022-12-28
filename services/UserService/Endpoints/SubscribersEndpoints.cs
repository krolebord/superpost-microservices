using System.Security.Claims;
using Common.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Services;

namespace UserService.Endpoints;

public static class SubscribersEndpoints
{
    public static async Task<IResult> GetUserSubscriptions(
        [FromRoute] Guid userId,
        UsersContext context)
    {
        var subscribedTo = await context.Users
            .Where(x => x.Id == userId)
            .SelectMany(x => x.SubscribedTo)
            .Select(UserSelectors.UserDto)
            .ToListAsync();

        return Results.Ok(subscribedTo);
    }
    
    public static async Task<IResult> GetUserSubscribers(
        [FromRoute] Guid userId,
        UsersContext context)
    {
        var subscriberIds = await context.Users
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Subscribers)
            .Select(UserSelectors.UserDto)
            .ToListAsync();

        return Results.Ok(subscriberIds);
    }

    public static async Task<IResult> CheckIfCurrentUserIsSubscribedTo(
        [FromRoute] Guid userId,
        UsersContext context,
        ClaimsPrincipal user)
    {
        var currentUserId = user.GetUserId();
        var result = await context.Subscriptions
            .AnyAsync(x => x.UserId == currentUserId && x.SubscribedToId == userId);
        return Results.Ok(result);
    }

    public static async Task<IResult> SubscribeCurrentUserTo(
        ClaimsPrincipal principal,
        SubscribersService subscribersService,
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid[]? userIds = null)
    {
        var subscriberId = principal.GetUserId();
        if (userId is not null)
        {
            await subscribersService.SubscribeUserTo(subscriberId, userId.Value);
            return Results.Ok();
        }
        if (userIds is not null && userIds.Length > 0)
        {
            await subscribersService.SubscribeUserTo(subscriberId, userIds);
            return Results.Ok();
        }
        return Results.BadRequest();
    }
    
    public static async Task<IResult> UnsubscribeCurrentUserFrom(
        ClaimsPrincipal principal,
        SubscribersService subscribersService,
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid[]? userIds = null)
    {
        var subscriberId = principal.GetUserId();
        if (userId is not null)
        {
            await subscribersService.UnsubscribeUserFrom(subscriberId, userId.Value);
            return Results.Ok();
        }
        if (userIds is not null && userIds.Length > 0)
        {
            await subscribersService.UnsubscribeUserFrom(subscriberId, userIds);
            return Results.Ok();
        }
        return Results.BadRequest();
    }
}
