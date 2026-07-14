namespace Greetly.Application.Interfaces.Agents;

public record DesignAgentRequest(string ContentText, string OccasionName, string StyleHint);

public interface IDesignAgentClient
{
    Task<string> GenerateSvgAsync(DesignAgentRequest request);
}
