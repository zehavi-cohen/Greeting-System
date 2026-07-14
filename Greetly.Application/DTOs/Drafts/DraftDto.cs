namespace Greetly.Application.DTOs.Drafts;

public record DraftDto(
    int Id,
    Guid? GreetingId,
    string DraftContent,
    int Version,
    DateTime CreatedAt);