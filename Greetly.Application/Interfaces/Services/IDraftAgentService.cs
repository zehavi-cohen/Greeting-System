using Greetly.Application.DTOs.Drafts;
using Greetly.Application.DTOs.Greetings;

namespace Greetly.Application.Interfaces.Services;

public interface IDraftAgentService
{
    Task<DraftDto> GenerateInitialDraftAsync(Guid userId, CreateDraftRequest request);
    Task<DraftDto?> ReviseDraftAsync(int draftId, Guid userId, ReviseDraftRequest request);
    Task<GreetingDto?> FinalizeDraftAsync(int draftId, Guid userId, FinalizeDraftRequest request);
}
