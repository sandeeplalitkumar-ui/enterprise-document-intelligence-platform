using System.Collections.Concurrent;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Infrastructure.Repositories;

public class InMemoryDocumentTextExtractionRepository : IDocumentTextExtractionRepository
{
    private readonly ConcurrentDictionary<Guid, DocumentTextExtraction> _extractionsByDocumentId = new();

    public Task<DocumentTextExtraction> AddAsync(DocumentTextExtraction extraction)
    {
        _extractionsByDocumentId[extraction.DocumentId] = extraction;

        return Task.FromResult(extraction);
    }

    public Task<DocumentTextExtraction?> GetByDocumentIdAsync(Guid documentId)
    {
        _extractionsByDocumentId.TryGetValue(documentId, out var extraction);

        return Task.FromResult(extraction);
    }
}