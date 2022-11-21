using UserService.Models;

namespace UserService.Dtos;

public record UserCreateDto(string Name, string Email)
{
    public User ToUser() => new()
    {
        Id = Guid.NewGuid(),
        Name = Name,
        Email = Email
    };
}
