using Greetly.Api.Extensions;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class FavoriteEndpoints
{
    public static void MapFavoriteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/greetings/{id:guid}/favorite")
            .WithTags("Favorites")
            .RequireAuthorization();

        group.MapPost("/", async (Guid id, IFavoriteService service, HttpContext ctx) =>
        {
            await service.AddAsync(id, ctx.GetUserId());
            return Results.NoContent();
        });

        group.MapDelete("/", async (Guid id, IFavoriteService service, HttpContext ctx) =>
        {
            await service.RemoveAsync(id, ctx.GetUserId());
            return Results.NoContent();
        });
    }
}
