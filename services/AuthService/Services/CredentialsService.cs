using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthService.Services;

public class CredentialsService
{
    public (string Salt, string Hash) CreatePasswordHash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        var hash = GetPasswordHash(password, salt);
        
        return (Convert.ToBase64String(salt), hash);
    }

    public string GetPasswordHash(string password, string salt)
    {
        return GetPasswordHash(password, Convert.FromBase64String(salt));
    }
    
    public string GetPasswordHash(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));
    }
}
