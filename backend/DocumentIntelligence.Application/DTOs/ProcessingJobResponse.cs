using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.DTOs;

public class ProcessingJobResponse
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid DocumentId { get; set; }

    public ProcessingJobStatus Status { get; set; }

    public int RetryCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? StartedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }
    public string RequestedBy { get; set; } = string.Empty;

}