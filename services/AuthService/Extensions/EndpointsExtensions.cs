using AuthService.Data;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services;
using Common.Clients.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Extensions;

public static class EndpointsExtensions
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/register", Register);
        
        app.MapPost("/login", Login);
        app.MapPost("/logout", Logout);

        app.MapGet("/check", [Authorize] () => Results.Ok());

        return app;
    }

    private static async Task<IResult> Register(
        [FromBody] CreateUserRegistrationDto registrationDto,
        UserServiceClient client,
        CredentialsService credentialsService,
        AuthContext context,
        JwtService jwtService,
        HttpResponse response)
    {
        var (salt, hash) = credentialsService.CreatePasswordHash(registrationDto.Password);

        var userId = await client.CreateUser(new(registrationDto.Username, registrationDto.Email));
            
        var credentials = new Credentials
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Email = registrationDto.Email,
            UserName = registrationDto.Username,
            PasswordSalt = salt,
            PasswordHash = hash
        };
        context.Credentials.Add(credentials);
        await context.SaveChangesAsync();

        var token = jwtService.CreateAccessToken(credentials);
        AppendAuthCookies(response, token);
        
        return Results.Ok();
    }

    private static async Task<IResult> Login(
        [FromBody] CreateUserLoginDto loginDto,
        CredentialsService credentialsService,
        JwtService jwtService,
        AuthContext context,
        HttpResponse response)
    {
        var credentials = await context.Credentials
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UsernameOrEmail || x.Email == loginDto.UsernameOrEmail);
        if (credentials is null) return Results.BadRequest();
            
        var passwordHash = credentialsService.GetPasswordHash(loginDto.Password, credentials.PasswordSalt);
        if (passwordHash != credentials.PasswordHash) return Results.BadRequest();

        var token = jwtService.CreateAccessToken(credentials);
        AppendAuthCookies(response, token);
        
        return Results.Ok();
    }

    private static IResult Logout(HttpResponse response)
    {
        DeleteAuthCookies(response);
        return Results.Ok();
    }

    private static void AppendAuthCookies(HttpResponse response, string token)
    {
        response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
    }

    private static void DeleteAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete("X-Access-Token");
    }
}
