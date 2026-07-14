using Greetly.Application.Interfaces.Services;
using Greetly.Domain.Entities;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _db;

    public FavoriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Guid greetingId, Guid userId)
    {
        var exists = await _db.GreetingFavorites.AnyAsync(f => f.UserId == userId && f.GreetingId == greetingId);
        if (exists) return;

        _db.GreetingFavorites.Add(new GreetingFavorite
        {
            UserId = userId,
            GreetingId = greetingId,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid greetingId, Guid userId)
    {
        var favorite = await _db.GreetingFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.GreetingId == greetingId);

        if (favorite is null) return;

        _db.GreetingFavorites.Remove(favorite);
        await _db.SaveChangesAsync();
    }
}
