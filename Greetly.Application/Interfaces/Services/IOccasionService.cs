using Greetly.Application.DTOs.Occasions;

namespace Greetly.Application.Interfaces.Services;

public interface IOccasionService
{
    Task<OccasionDto> CreateAsync(CreateOccasionRequest request);
    Task<OccasionDto?> UpdateAsync(int id, UpdateOccasionRequest request);
    Task<bool> DeactivateAsync(int id);
}
