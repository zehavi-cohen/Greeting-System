namespace Greetly.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<Greeting> Greetings { get; set; } = new List<Greeting>();
    public ICollection<GreetingFavorite> Favorites { get; set; } = new List<GreetingFavorite>();
    public ICollection<GreetingDraft> Drafts { get; set; } = new List<GreetingDraft>();
}