using Greetly.Application.DTOs.Occasions;

namespace Greetly.Application.Interfaces.Services;

public interface IOccasionCacheService
{
    IReadOnlyList<OccasionDto> GetAll();
    OccasionDto? GetById(int id);
    Task RefreshAsync();
}
