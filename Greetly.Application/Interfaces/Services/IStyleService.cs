using Greetly.Application.DTOs.Styles;

namespace Greetly.Application.Interfaces.Services;

public interface IStyleService
{
    Task<List<StyleDto>> GetAllActiveAsync();
    Task<StyleDto> CreateAsync(CreateStyleRequest request);
    Task<StyleDto?> UpdateAsync(int id, UpdateStyleRequest request);
}
