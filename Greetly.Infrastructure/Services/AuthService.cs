using Greetly.Application.DTOs.Auth;
using Greetly.Application.Interfaces.Auth;
using Greetly.Application.Interfaces.Services;
using Greetly.Domain.Entities;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(AppDbContext db, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username);
        if (exists) return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            DisplayName = request.DisplayName,
            PasswordHash = _passwordHasher.Hash(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.DisplayName);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            return null;

        var token = _jwtTokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.DisplayName);
    }
}
