using Greetly.Domain.Entities;

namespace Greetly.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
