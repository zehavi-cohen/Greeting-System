using Greetly.Domain.Enums;

namespace Greetly.Domain.Entities;

public class Greeting
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int OccasionId { get; set; }
    public Occasion Occasion { get; set; } = null!;

    public int StyleId { get; set; }
    public Style Style { get; set; } = null!;

    public string RecipientName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string ContentText { get; set; } = null!;
    public string? DesignSvgUrl { get; set; }

    public Visibility Visibility { get; set; } = Visibility.Private;
    public int ViewCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<GreetingFavorite> Favorites { get; set; } = new List<GreetingFavorite>();
    public ICollection<GreetingDraft> Drafts { get; set; } = new List<GreetingDraft>();
}