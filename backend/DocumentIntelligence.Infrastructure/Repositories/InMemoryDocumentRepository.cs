using System.Collections.Concurrent;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Infrastructure.Repositories;

public class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ConcurrentDictionary<Guid, Document> _documents = new();

    public Task<Document> AddAsync(Document document)
    {
        _documents[document.Id] = document;

        return Task.FromResult(document);
    }

    public Task<IReadOnlyList<Document>> GetByTenantIdAsync(Guid tenantId)
    {
        IReadOnlyList<Document> documents = _documents.Values
            .Where(d => d.TenantId == tenantId)
            .OrderByDescending(d => d.UploadedAtUtc)
            .ToList();

        return Task.FromResult(documents);
    }

    public Task<Document?> GetByIdAsync(Guid documentId)
    {
        _documents.TryGetValue(documentId, out var document);

        return Task.FromResult(document);
    }

    public Task UpdateAsync(Document document)
    {
        _documents[document.Id] = document;
        return Task.CompletedTask;
    }
}