using Greetly.Application.Interfaces.Agents;

namespace Greetly.Infrastructure.Agents;

public class DesignAgentClient : IDesignAgentClient
{
    public Task<string> GenerateSvgAsync(DesignAgentRequest request) =>
        Task.FromResult("<svg viewBox='0 0 400 300' xmlns='http://www.w3.org/2000/svg'><rect width='400' height='300' fill='#fdf6e3'/><text x='50%' y='50%' text-anchor='middle'>TODO: עיצוב אמיתי</text></svg>");
}