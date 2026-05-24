using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Application.Interfaces;

public interface IDocumentRepository
{
    Task<Document> AddAsync(Document document);

    Task<IReadOnlyList<Document>> GetByTenantIdAsync(Guid tenantId);

    Task<Document?> GetByIdAsync(Guid documentId);
}