using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Endpoints;

public static class UserEndpoints
{
    public static async Task<IResult> GetUsers(
        UsersContext context,
        [FromQuery] Guid[] ids)
    {
        var users = await context.Users
            .Where(x => ids.Contains(x.Id))
            .Select(UserSelectors.UserDto)
            .ToListAsync();
        return Results.Ok(users);
    }
    
    public static async Task<IResult> CreateUser(
        [FromBody] UserCreateDto userDto,
        UsersContext context)
    {
        var user = userDto.ToUser(Guid.NewGuid());
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Results.Ok(user.Id);
    }
}
