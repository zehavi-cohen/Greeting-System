using Greetly.Application.DTOs.Occasions;
using Greetly.Application.DTOs.Styles;
using Greetly.Domain.Enums;

namespace Greetly.Application.DTOs.Greetings;

public record GreetingDto(
    Guid Id,
    string RecipientName,
    string Title,
    string ContentText,
    string? DesignSvgUrl,
    OccasionDto Occasion,
    StyleDto Style,
    Visibility Visibility,
    int ViewCount,
    int FavoritesCount,
    bool IsFavoritedByCurrentUser,
    DateTime CreatedAt);
