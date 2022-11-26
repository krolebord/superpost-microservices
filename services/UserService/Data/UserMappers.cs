using UserService.Models;

namespace UserService.Data;

public static class UserMappers
{
    public static User ToUser(this UserCreateDto dto, Guid id) => new()
    {
        Id = id,
        Email = dto.Email,
        Name = dto.Name
    };
}
