using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.DTOs;

public class CreateDocumentRequest
{
    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long SizeInBytes { get; set; }

    public string StoragePath { get; set; } = string.Empty;

    public DocumentClassification Classification { get; set; } = DocumentClassification.Internal;
}