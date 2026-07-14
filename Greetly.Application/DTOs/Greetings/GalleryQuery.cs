namespace Greetly.Application.DTOs.Greetings;

public record GalleryQuery(
    int? OccasionId,
    int? StyleId,
    string? SearchTerm,
    int Page = 1,
    int PageSize = 12);
