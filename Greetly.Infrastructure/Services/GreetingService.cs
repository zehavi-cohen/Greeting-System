using Greetly.Application.DTOs.Greetings;
using Greetly.Application.DTOs.Occasions;
using Greetly.Application.DTOs.Styles;
using Greetly.Application.Interfaces.Services;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class GreetingService : IGreetingService
{
    private readonly AppDbContext _db;

    public GreetingService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<GreetingDto?> GetByIdAsync(Guid id, Guid? currentUserId)
    {
        var greeting = await _db.Greetings
            .Include(g => g.Occasion)
            .Include(g => g.Style)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (greeting is null) return null;

        var favoritesCount = await _db.GreetingFavorites.CountAsync(f => f.GreetingId == id);
        var isFavorited = currentUserId is not null &&
            await _db.GreetingFavorites.AnyAsync(f => f.UserId == currentUserId && f.GreetingId == id);

        return new GreetingDto(
            greeting.Id,
            greeting.RecipientName,
            greeting.Title,
            greeting.ContentText,
            greeting.DesignSvgUrl,
            new OccasionDto(greeting.Occasion.Id, greeting.Occasion.Name),
            new StyleDto(greeting.Style.Id, greeting.Style.Name),
            greeting.Visibility,
            greeting.ViewCount,
            favoritesCount,
            isFavorited,
            greeting.CreatedAt);
    }

    public async Task<List<GreetingSummaryDto>> GetByUserAsync(Guid userId)
    {
        return await _db.Greetings
            .Include(g => g.Occasion)
            .Where(g => g.UserId == userId)
            .Select(g => new GreetingSummaryDto(
                g.Id, g.Title, g.RecipientName, g.DesignSvgUrl, g.Occasion.Name,
                g.Favorites.Count, g.CreatedAt))
            .ToListAsync();
    }

    public async Task<List<GreetingSummaryDto>> GetGalleryAsync(GalleryQuery query)
    {
        var q = _db.Greetings
            .Include(g => g.Occasion)
            .Where(g => g.Visibility == Domain.Enums.Visibility.Public);

        if (query.OccasionId is not null)
            q = q.Where(g => g.OccasionId == query.OccasionId);

        if (query.StyleId is not null)
            q = q.Where(g => g.StyleId == query.StyleId);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            q = q.Where(g => g.Title.Contains(query.SearchTerm) || g.RecipientName.Contains(query.SearchTerm));

        return await q
            .OrderByDescending(g => g.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(g => new GreetingSummaryDto(
                g.Id, g.Title, g.RecipientName, g.DesignSvgUrl, g.Occasion.Name,
                g.Favorites.Count, g.CreatedAt))
            .ToListAsync();
    }

    public async Task<GreetingDto?> UpdateAsync(Guid id, Guid userId, UpdateGreetingRequest request)
    {
        var greeting = await _db.Greetings.FirstOrDefaultAsync(g => g.Id == id);
        if (greeting is null || greeting.UserId != userId) return null;

        greeting.RecipientName = request.RecipientName;
        greeting.Title = request.Title;
        greeting.ContentText = request.ContentText;
        greeting.Visibility = request.Visibility;
        greeting.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return await GetByIdAsync(id, userId);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var greeting = await _db.Greetings.FirstOrDefaultAsync(g => g.Id == id);
        if (greeting is null || greeting.UserId != userId) return false;

        _db.Greetings.Remove(greeting);
        await _db.SaveChangesAsync();
        return true;
    }
}
