namespace Greetly.Application.Interfaces.Storage;

public interface IFileStorageService
{
    Task<string> SaveSvgAsync(Guid greetingId, string svgContent);
}
