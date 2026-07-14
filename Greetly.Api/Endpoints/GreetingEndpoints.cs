using Greetly.Api.Extensions;
using Greetly.Application.DTOs.Greetings;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class GreetingEndpoints
{
    public static void MapGreetingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/greetings")
            .WithTags("Greetings")
            .RequireAuthorization();

        app.MapGet("/api/gallery", async ([AsParameters] GalleryQuery query, IGreetingService service) =>
            Results.Ok(await service.GetGalleryAsync(query)))
            .WithTags("Greetings")
            .AllowAnonymous();

        group.MapGet("/{id:guid}", async (Guid id, IGreetingService service, HttpContext ctx) =>
        {
            var greeting = await service.GetByIdAsync(id, ctx.GetUserId());
            return greeting is not null ? Results.Ok(greeting) : Results.NotFound();
        });

        group.MapGet("/mine", async (IGreetingService service, HttpContext ctx) =>
            Results.Ok(await service.GetByUserAsync(ctx.GetUserId())));

        group.MapPut("/{id:guid}", async (Guid id, UpdateGreetingRequest request, IGreetingService service, HttpContext ctx) =>
        {
            var updated = await service.UpdateAsync(id, ctx.GetUserId(), request);
            return updated is not null ? Results.Ok(updated) : Results.Forbid();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IGreetingService service, HttpContext ctx) =>
        {
            var success = await service.DeleteAsync(id, ctx.GetUserId());
            return success ? Results.NoContent() : Results.Forbid();
        });
    }
}
