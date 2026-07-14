using Greetly.Application.DTOs.Auth;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterRequest request, IAuthService service) =>
        {
            var result = await service.RegisterAsync(request);
            return result is not null ? Results.Ok(result) : Results.BadRequest("User already exists");
        });

        group.MapPost("/login", async (LoginRequest request, IAuthService service) =>
        {
            var result = await service.LoginAsync(request);
            return result is not null ? Results.Ok(result) : Results.Unauthorized();
        });
    }
}
