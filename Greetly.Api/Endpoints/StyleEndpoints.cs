using Greetly.Application.DTOs.Styles;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class StyleEndpoints
{
    public static void MapStyleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/styles").WithTags("Styles");

        group.MapGet("/", async (IStyleService service) => Results.Ok(await service.GetAllActiveAsync()));

        var admin = app.MapGroup("/api/admin/styles")
            .WithTags("Styles - Admin")
            .RequireAuthorization("AdminOnly");

        admin.MapPost("/", async (CreateStyleRequest request, IStyleService service) =>
        {
            var created = await service.CreateAsync(request);
            return Results.Created($"/api/styles/{created.Id}", created);
        });

        admin.MapPut("/{id:int}", async (int id, UpdateStyleRequest request, IStyleService service) =>
        {
            var updated = await service.UpdateAsync(id, request);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });
    }
}
