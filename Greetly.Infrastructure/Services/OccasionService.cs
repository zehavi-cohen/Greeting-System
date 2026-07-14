using Greetly.Application.DTOs.Occasions;
using Greetly.Application.Interfaces.Services;
using Greetly.Domain.Entities;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class OccasionService : IOccasionService
{
    private readonly AppDbContext _db;
    private readonly IOccasionCacheService _cache;

    public OccasionService(AppDbContext db, IOccasionCacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<OccasionDto> CreateAsync(CreateOccasionRequest request)
    {
        var occasion = new Occasion { Name = request.Name, SortOrder = request.SortOrder, IsActive = true };
        _db.Occasions.Add(occasion);
        await _db.SaveChangesAsync();
        await _cache.RefreshAsync();

        return new OccasionDto(occasion.Id, occasion.Name);
    }

    public async Task<OccasionDto?> UpdateAsync(int id, UpdateOccasionRequest request)
    {
        var occasion = await _db.Occasions.FindAsync(id);
        if (occasion is null) return null;

        occasion.Name = request.Name;
        occasion.SortOrder = request.SortOrder;
        occasion.IsActive = request.IsActive;

        await _db.SaveChangesAsync();
        await _cache.RefreshAsync();

        return new OccasionDto(occasion.Id, occasion.Name);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var occasion = await _db.Occasions.FindAsync(id);
        if (occasion is null) return false;

        occasion.IsActive = false;
        await _db.SaveChangesAsync();
        await _cache.RefreshAsync();

        return true;
    }
}
