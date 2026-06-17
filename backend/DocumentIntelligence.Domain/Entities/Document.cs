using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Domain.Entities;

public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TenantId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long SizeInBytes { get; set; }

    public string StoragePath { get; set; } = string.Empty;

    public DocumentClassification Classification { get; set; } = DocumentClassification.Internal;

    public DocumentStatus Status { get; set; } = DocumentStatus.Uploaded;

    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAtUtc { get; set; }

    public List<DocumentChunk> Chunks { get; set; } = new();

    public void MarkAsProcessed()
    {
        Status = DocumentStatus.Processed;
        ProcessedAtUtc = DateTime.UtcNow;
    }
}