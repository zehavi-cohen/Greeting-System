namespace Greetly.Domain.Entities;

public class Occasion
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<Greeting> Greetings { get; set; } = new List<Greeting>();
}