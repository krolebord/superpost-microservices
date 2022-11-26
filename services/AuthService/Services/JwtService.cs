using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthService.Models;
using Common.Auth.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class JwtService
{
    private readonly JwtOptions _options;

    private RSAParameters _privateParameters;

    public JwtService(JwtOptions options)
    {
        _options = options;

        if (_options.PrivateKey is null)
        {
            throw new Exception("Private JWT key is not provided");
        }
        
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(
            source: Convert.FromBase64String(options.PrivateKey!),
            bytesRead: out int _
        );
        _privateParameters = rsa.ExportParameters(true);
    }
    public string CreateAccessToken(Credentials credentials)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Email, credentials.Email),
            new(ClaimTypes.Name, credentials.UserName),
            new(ClaimTypes.NameIdentifier, credentials.UserId.ToString())
        };
        

        var key = new RsaSecurityKey(_privateParameters);
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            // expires: DateTime.UtcNow + _options.Expire,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
