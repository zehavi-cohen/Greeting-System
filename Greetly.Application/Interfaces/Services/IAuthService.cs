using Greetly.Application.DTOs.Auth;

namespace Greetly.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}
