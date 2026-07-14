using Greetly.Application.DTOs.Occasions;
using Greetly.Application.Interfaces.Services;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Greetly.Infrastructure.Services;

public class OccasionCacheService : IOccasionCacheService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IReadOnlyList<OccasionDto> _cache = new List<OccasionDto>();

    public OccasionCacheService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IReadOnlyList<OccasionDto> GetAll() => _cache;

    public OccasionDto? GetById(int id) => _cache.FirstOrDefault(o => o.Id == id);

    public async Task RefreshAsync()
    {
        await _lock.WaitAsync();
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            _cache = await db.Occasions
                .Where(o => o.IsActive)
                .OrderBy(o => o.SortOrder)
                .Select(o => new OccasionDto(o.Id, o.Name))
                .ToListAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
}
