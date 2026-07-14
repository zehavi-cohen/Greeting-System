namespace Greetly.Domain.Entities;

public class GreetingDraft
{
    public int Id { get; set; }

    public Guid? GreetingId { get; set; }
    public Greeting? Greeting { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int OccasionId { get; set; }
    public Occasion Occasion { get; set; } = null!;

    public int StyleId { get; set; }
    public Style Style { get; set; } = null!;

    public string RecipientName { get; set; } = null!;
    public string RawUserText { get; set; } = null!;
    public string? UserInstruction { get; set; }
    public string DraftContent { get; set; } = null!;
    public int Version { get; set; }

    public DateTime CreatedAt { get; set; }
}