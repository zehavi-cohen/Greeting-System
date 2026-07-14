namespace Greetly.Domain.Entities;

public class Style
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string AgentPromptHint { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Greeting> Greetings { get; set; } = new List<Greeting>();
}