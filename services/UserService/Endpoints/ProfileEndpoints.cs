using System.Security.Claims;
using Common.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Endpoints;

public static class ProfileEndpoints
{
    public static async Task<IResult> GetProfileById(
        [FromRoute] string idOrName,
        UsersContext context)
    {
        IQueryable<User> userQuery = context.Users;

        userQuery = Guid.TryParse(idOrName, out var id)
            ? userQuery.Where(x => x.Id == id)
            : userQuery.Where(x => x.Name == idOrName);

        var user = await userQuery
            .Select(UserSelectors.ProfileDto)
            .FirstOrDefaultAsync();
        if (user is null) return Results.NotFound();
        return Results.Ok(user);
    }

    public static Task<IResult> GetCurrentUserProfile(
        ClaimsPrincipal user,
        UsersContext context)
    {
        var id = user.GetUserId();
        return GetProfileById(id.ToString(), context);
    }
}
