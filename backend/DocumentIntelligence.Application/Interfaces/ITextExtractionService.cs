using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Application.Interfaces;

public interface ITextExtractionService
{
    Task<string> ExtractTextAsync(
        Document document,
        CancellationToken cancellationToken = default);
}