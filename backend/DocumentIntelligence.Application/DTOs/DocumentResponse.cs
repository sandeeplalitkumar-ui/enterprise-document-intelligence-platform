using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.DTOs;

public class DocumentResponse
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long SizeInBytes { get; set; }

    public string StoragePath { get; set; } = string.Empty;

    public DocumentClassification Classification { get; set; }

    public DocumentStatus Status { get; set; }

    public DateTime UploadedAtUtc { get; set; }

    public DateTime? ProcessedAtUtc { get; set; }
}