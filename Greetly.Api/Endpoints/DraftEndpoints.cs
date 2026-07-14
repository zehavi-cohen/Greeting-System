using Greetly.Api.Extensions;
using Greetly.Application.DTOs.Drafts;
using Greetly.Application.Interfaces.Services;

namespace Greetly.Api.Endpoints;

public static class DraftEndpoints
{
    public static void MapDraftEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/drafts").WithTags("Drafts").RequireAuthorization();

        group.MapPost("/", async (CreateDraftRequest request, IDraftAgentService agentService, HttpContext ctx) =>
        {
            var draft = await agentService.GenerateInitialDraftAsync(ctx.GetUserId(), request);
            return Results.Created($"/api/drafts/{draft.Id}", draft);
        });

        group.MapPost("/{id:int}/revise", async (int id, ReviseDraftRequest request, IDraftAgentService agentService, HttpContext ctx) =>
        {
            var revised = await agentService.ReviseDraftAsync(id, ctx.GetUserId(), request);
            return revised is not null ? Results.Ok(revised) : Results.NotFound();
        });

        group.MapPost("/{id:int}/finalize", async (int id, FinalizeDraftRequest request, IDraftAgentService agentService, HttpContext ctx) =>
        {
            var greeting = await agentService.FinalizeDraftAsync(id, ctx.GetUserId(), request);
            return greeting is not null ? Results.Ok(greeting) : Results.NotFound();
        });
    }
}
