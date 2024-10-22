using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TipsRundan.Application.Interfaces;

namespace TipsRundan.Infrastructure.Security;

public class CryptographyService : ICryptographyService
{
    public string HashPassword(string passwordString)
    {
        var saltBytes = Encoding.UTF8.GetBytes("OurStr0ngSalt");
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: passwordString,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            )
        );
    }

    public bool Validate(string potentialPasswordString, string actualPasswordHash)
    {
        return string.Equals(HashPassword(potentialPasswordString), actualPasswordHash);
    }
}