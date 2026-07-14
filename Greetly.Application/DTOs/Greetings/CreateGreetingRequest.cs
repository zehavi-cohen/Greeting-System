using Greetly.Domain.Enums;

namespace Greetly.Application.DTOs.Greetings;

public record CreateGreetingRequest(
    string RecipientName,
    string Title,
    string ContentText,
    int OccasionId,
    int StyleId,
    Visibility Visibility);
