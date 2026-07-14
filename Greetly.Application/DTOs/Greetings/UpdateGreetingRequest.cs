using Greetly.Domain.Enums;

namespace Greetly.Application.DTOs.Greetings;

public record UpdateGreetingRequest(
    string RecipientName,
    string Title,
    string ContentText,
    Visibility Visibility);
