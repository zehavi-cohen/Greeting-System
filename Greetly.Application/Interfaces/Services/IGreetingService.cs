using Greetly.Application.DTOs.Greetings;

namespace Greetly.Application.Interfaces.Services;

public interface IGreetingService
{
    Task<GreetingDto?> GetByIdAsync(Guid id, Guid? currentUserId);
    Task<List<GreetingSummaryDto>> GetByUserAsync(Guid userId);
    Task<List<GreetingSummaryDto>> GetGalleryAsync(GalleryQuery query);
    Task<GreetingDto?> UpdateAsync(Guid id, Guid userId, UpdateGreetingRequest request);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
