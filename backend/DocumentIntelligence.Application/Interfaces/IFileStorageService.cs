namespace DocumentIntelligence.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        Guid tenantId);
}