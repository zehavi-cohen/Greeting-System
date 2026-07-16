using Greetly.Application.Interfaces.Agents;

namespace Greetly.Infrastructure.Agents;

public class ContentAgentClient : IContentAgentClient
{
    public Task<string> GenerateAsync(ContentAgentRequest request) =>
        Task.FromResult($"ברכה חמה ל{request.RecipientName} לרגל {request.OccasionName} — טקסט זמני (TODO: לחבר AI אמיתי)");

    public Task<string> ReviseAsync(ContentAgentReviseRequest request) =>
        Task.FromResult(request.CurrentContent + " (מעודכן — TODO: לחבר AI אמיתי)");
}