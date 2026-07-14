using Greetly.Application.DTOs.Styles;
using Greetly.Application.Interfaces.Services;
using Greetly.Domain.Entities;
using Greetly.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Services;

public class StyleService : IStyleService
{
    private readonly AppDbContext _db;

    public StyleService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<StyleDto>> GetAllActiveAsync()
    {
        return await _db.Styles
            .Where(s => s.IsActive)
            .Select(s => new StyleDto(s.Id, s.Name))
            .ToListAsync();
    }

    public async Task<StyleDto> CreateAsync(CreateStyleRequest request)
    {
        var style = new Style
        {
            Name = request.Name,
            AgentPromptHint = request.AgentPromptHint,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Styles.Add(style);
        await _db.SaveChangesAsync();

        return new StyleDto(style.Id, style.Name);
    }

    public async Task<StyleDto?> UpdateAsync(int id, UpdateStyleRequest request)
    {
        var style = await _db.Styles.FindAsync(id);
        if (style is null) return null;

        style.Name = request.Name;
        style.AgentPromptHint = request.AgentPromptHint;
        style.IsActive = request.IsActive;
        style.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new StyleDto(style.Id, style.Name);
    }
}
