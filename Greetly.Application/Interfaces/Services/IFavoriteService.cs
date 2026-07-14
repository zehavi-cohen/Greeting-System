namespace Greetly.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task AddAsync(Guid greetingId, Guid userId);
    Task RemoveAsync(Guid greetingId, Guid userId);
}
