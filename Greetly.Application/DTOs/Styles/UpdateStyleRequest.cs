namespace Greetly.Application.DTOs.Styles;

public record UpdateStyleRequest(string Name, string AgentPromptHint, bool IsActive);
