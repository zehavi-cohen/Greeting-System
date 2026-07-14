namespace Greetly.Application.Interfaces.Agents;

public record ContentAgentRequest(string RawUserText, string OccasionName, string StyleHint, string RecipientName);
public record ContentAgentReviseRequest(string CurrentContent, string UserInstruction, string StyleHint);

public interface IContentAgentClient
{
    Task<string> GenerateAsync(ContentAgentRequest request);
    Task<string> ReviseAsync(ContentAgentReviseRequest request);
}
