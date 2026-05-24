using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.DTOs;

public class UploadDocumentRequest
{
    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long SizeInBytes { get; set; }

    public Stream FileStream { get; set; } = Stream.Null;

    public DocumentClassification Classification { get; set; } = DocumentClassification.Internal;
}