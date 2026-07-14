using Greetly.Application.DTOs.Drafts;
using Greetly.Application.DTOs.Greetings;
using Greetly.Application.Interfaces.Agents;
using Greetly.Application.Interfaces.Services;
using Greetly.Application.Interfaces.Storage;
using Greetly.Domain.Entities;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class DraftAgentService : IDraftAgentService
{
    private readonly AppDbContext _db;
    private readonly IContentAgentClient _contentAgent;
    private readonly IDesignAgentClient _designAgent;
    private readonly IFileStorageService _fileStorage;
    private readonly IGreetingService _greetingService;

    public DraftAgentService(
        AppDbContext db,
        IContentAgentClient contentAgent,
        IDesignAgentClient designAgent,
        IFileStorageService fileStorage,
        IGreetingService greetingService)
    {
        _db = db;
        _contentAgent = contentAgent;
        _designAgent = designAgent;
        _fileStorage = fileStorage;
        _greetingService = greetingService;
    }

    public async Task<DraftDto> GenerateInitialDraftAsync(Guid userId, CreateDraftRequest request)
    {
        var occasion = await _db.Occasions.FindAsync(request.OccasionId)
            ?? throw new InvalidOperationException("Occasion not found");
        var style = await _db.Styles.FindAsync(request.StyleId)
            ?? throw new InvalidOperationException("Style not found");

        var content = await _contentAgent.GenerateAsync(new ContentAgentRequest(
            request.RawUserText, occasion.Name, style.AgentPromptHint, request.RecipientName));

        var draft = new GreetingDraft
        {
            UserId = userId,
            OccasionId = request.OccasionId,
            StyleId = request.StyleId,
            RecipientName = request.RecipientName,
            RawUserText = request.RawUserText,
            DraftContent = content,
            Version = 1,
            CreatedAt = DateTime.UtcNow
        };

        _db.GreetingDrafts.Add(draft);
        await _db.SaveChangesAsync();

        return new DraftDto(draft.Id, draft.GreetingId, draft.DraftContent, draft.Version, draft.CreatedAt);
    }

    public async Task<DraftDto?> ReviseDraftAsync(int draftId, Guid userId, ReviseDraftRequest request)
    {
        var current = await _db.GreetingDrafts
            .Include(d => d.Style)
            .FirstOrDefaultAsync(d => d.Id == draftId);

        if (current is null || current.UserId != userId) return null;

        var revisedContent = await _contentAgent.ReviseAsync(
            new ContentAgentReviseRequest(current.DraftContent, request.UserInstruction, current.Style.AgentPromptHint));

        var newDraft = new GreetingDraft
        {
            UserId = userId,
            OccasionId = current.OccasionId,
            StyleId = current.StyleId,
            RecipientName = current.RecipientName,
            RawUserText = current.RawUserText,
            UserInstruction = request.UserInstruction,
            DraftContent = revisedContent,
            Version = current.Version + 1,
            CreatedAt = DateTime.UtcNow
        };

        _db.GreetingDrafts.Add(newDraft);
        await _db.SaveChangesAsync();

        return new DraftDto(newDraft.Id, newDraft.GreetingId, newDraft.DraftContent, newDraft.Version, newDraft.CreatedAt);
    }

    public async Task<GreetingDto?> FinalizeDraftAsync(int draftId, Guid userId, FinalizeDraftRequest request)
    {
        var draft = await _db.GreetingDrafts
            .Include(d => d.Occasion)
            .Include(d => d.Style)
            .FirstOrDefaultAsync(d => d.Id == draftId);

        if (draft is null || draft.UserId != userId) return null;

        var greeting = new Greeting
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OccasionId = draft.OccasionId,
            StyleId = draft.StyleId,
            RecipientName = draft.RecipientName,
            Title = $"ברכת {draft.Occasion.Name} עבור {draft.RecipientName}",
            ContentText = draft.DraftContent,
            Visibility = request.Visibility,
            ViewCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var svg = await _designAgent.GenerateSvgAsync(
            new DesignAgentRequest(draft.DraftContent, draft.Occasion.Name, draft.Style.AgentPromptHint));
        greeting.DesignSvgUrl = await _fileStorage.SaveSvgAsync(greeting.Id, svg);

        _db.Greetings.Add(greeting);
        draft.GreetingId = greeting.Id;

        await _db.SaveChangesAsync();

        return await _greetingService.GetByIdAsync(greeting.Id, userId);
    }
}
