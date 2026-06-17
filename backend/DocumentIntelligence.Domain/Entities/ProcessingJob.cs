using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Domain.Entities;

public class ProcessingJob
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TenantId { get; set; }

    public Guid DocumentId { get; set; }

    public ProcessingJobStatus Status { get; set; } = ProcessingJobStatus.Pending;
    public string? ErrorMessage { get; private set; }
    public int RetryCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAtUtc { get; set; }

    public DateTime? CompletedAtUtc { get; set; }
    public string RequestedBy { get; set; } = string.Empty;

    public void MarkAsProcessing()
    {
        Status = ProcessingJobStatus.Processing;
        StartedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsSucceeded()
    {
        Status = ProcessingJobStatus.Succeeded;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = ProcessingJobStatus.Failed;
        ErrorMessage = errorMessage;
        CompletedAtUtc = DateTime.UtcNow;
    }
}