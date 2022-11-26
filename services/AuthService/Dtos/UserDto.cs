namespace AuthService.Dtos;

public record CreateUserLoginDto(string UsernameOrEmail, string Password);

public record CreateUserRegistrationDto(string Username, string Email, string Password);
