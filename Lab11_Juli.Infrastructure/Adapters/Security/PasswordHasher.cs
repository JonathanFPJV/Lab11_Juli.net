using Lab11_Juli.Domain.Ports.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Lab11_Juli.Infrastructure.Adapters.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    public bool Verify(string hash, string providedPassword)
    {
        return BCryptNet.Verify(providedPassword, hash);
    }
}