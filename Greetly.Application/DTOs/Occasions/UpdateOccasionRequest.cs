namespace Greetly.Application.DTOs.Occasions;

public record UpdateOccasionRequest(string Name, int SortOrder, bool IsActive);
