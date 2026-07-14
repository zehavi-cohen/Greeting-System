namespace Greetly.Application.DTOs.Drafts;

public record CreateDraftRequest(
    string RawUserText,
    int OccasionId,
    int StyleId,
    string RecipientName);
