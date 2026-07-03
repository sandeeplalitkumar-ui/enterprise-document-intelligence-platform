using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Application.Interfaces;

public interface IDocumentTextExtractionRepository
{
    Task<DocumentTextExtraction> AddAsync(DocumentTextExtraction extraction);

    Task<DocumentTextExtraction?> GetByDocumentIdAsync(Guid documentId);
}
