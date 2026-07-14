namespace Greetly.Application.DTOs.Greetings;

public record GreetingSummaryDto(
    Guid Id,
    string Title,
    string RecipientName,
    string? DesignSvgUrl,
    string OccasionName,
    int FavoritesCount,
    DateTime CreatedAt);
