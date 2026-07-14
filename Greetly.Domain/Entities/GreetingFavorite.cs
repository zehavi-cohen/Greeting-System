namespace Greetly.Domain.Entities;

public class GreetingFavorite
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid GreetingId { get; set; }
    public Greeting Greeting { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}