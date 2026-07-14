using Greetly.Application.DTOs.Occasions;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class OccasionEndpoints
{
    public static void MapOccasionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/occasions").WithTags("Occasions");

        group.MapGet("/", (IOccasionCacheService cache) => Results.Ok(cache.GetAll()));

        var admin = app.MapGroup("/api/admin/occasions")
            .WithTags("Occasions - Admin")
            .RequireAuthorization("AdminOnly");

        admin.MapPost("/", async (CreateOccasionRequest request, IOccasionService service) =>
        {
            var created = await service.CreateAsync(request);
            return Results.Created($"/api/occasions/{created.Id}", created);
        });

        admin.MapPut("/{id:int}", async (int id, UpdateOccasionRequest request, IOccasionService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        admin.MapDelete("/{id:int}", async (int id, IOccasionService service) =>
        {
            var success = await service.DeactivateAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
