using Lab11_Juli.Infrastructure.Data;

namespace Lab11_Juli.Domain.Ports.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles);
}