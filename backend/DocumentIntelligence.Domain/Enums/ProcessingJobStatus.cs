namespace DocumentIntelligence.Domain.Enums;

public enum ProcessingJobStatus
{
    Pending = 1,
    Processing = 2,
    Succeeded = 3,
    Failed = 4,
    DeadLettered = 5
}