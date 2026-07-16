using Greetly.Application.Interfaces.Storage;
using Microsoft.Extensions.Configuration;

namespace Greetly.Infrastructure.Storage;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public FileStorageService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"] ?? "wwwroot/svgs";
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveSvgAsync(Guid greetingId, string svgContent)
    {
        var fileName = $"{greetingId}.svg";
        var fullPath = Path.Combine(_basePath, fileName);
        await File.WriteAllTextAsync(fullPath, svgContent);
        return $"/svgs/{fileName}"; // ה-URL היחסי שנשמר ב-Greeting.DesignSvgUrl
    }
}