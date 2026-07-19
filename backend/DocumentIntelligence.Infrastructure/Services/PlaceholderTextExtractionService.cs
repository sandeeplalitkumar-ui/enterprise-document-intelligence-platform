using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Infrastructure.Services;

public class PlaceholderTextExtractionService : ITextExtractionService
{
    public Task<string> ExtractTextAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        var extractedText =
            $"Extracted text placeholder for document: {document.FileName}";

        return Task.FromResult(extractedText);
    }
}