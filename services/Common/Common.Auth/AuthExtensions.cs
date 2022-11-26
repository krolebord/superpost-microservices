using System.Security;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using Common.Auth.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Common.Auth;

public static class AuthExtensions
{
    public static JwtOptions GetJwtOptions(this IConfiguration configuration)
    { 
        return configuration
            .GetSection("Jwt")
            .Get<JwtOptions>()
               ?? throw new SecurityException("Jwt settings were not specified");
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, JwtOptions jwtOptions)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(
            source: Convert.FromBase64String(jwtOptions.PublicKey),
            bytesRead: out int _
        );

        var parameters = rsa.ExportParameters(false);
        
        services.TryAddSingleton(jwtOptions);
        
        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    
                    // TODO Validate lifetime and implement token refresh
                    ValidateLifetime = false,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new RsaSecurityKey(parameters)
                };
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => {

                        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie(options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });

        services.AddAuthorization();
        
        return services;
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        return Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new AuthenticationException("Missing NameIdentifier claim"));
    }
    
    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Name) ?? throw new AuthenticationException("Missing Name claim");
    }
    
    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email) ?? throw new AuthenticationException("Missing Email claim");
    }
}
