using DocumentIntelligence.Application.Interfaces;

namespace DocumentIntelligence.Infrastructure.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "storage", "documents");
    }

    public async Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        Guid tenantId)
    {
        var safeFileName = Path.GetFileName(fileName);

        var tenantFolder = Path.Combine(_basePath, tenantId.ToString());

        Directory.CreateDirectory(tenantFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";

        var fullPath = Path.Combine(tenantFolder, uniqueFileName);
        Console.WriteLine($"Saving uploaded file to: {fullPath}");

        await using var outputStream = new FileStream(fullPath, FileMode.CreateNew);

        await fileStream.CopyToAsync(outputStream);

        return Path.Combine("storage", "documents", tenantId.ToString(), uniqueFileName);
    }
}