using BCrypt.Net;
using Users.Application.Abstractions;

namespace Users.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);

        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        ArgumentException.ThrowIfNullOrEmpty(hashedPassword);
        ArgumentException.ThrowIfNullOrEmpty(providedPassword);

        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
        }
        catch (SaltParseException)
        {
            return false;
        }
    }
}
