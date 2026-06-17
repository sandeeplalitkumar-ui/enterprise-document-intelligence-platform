using DocumentIntelligence.Application.DTOs;

namespace DocumentIntelligence.Application.Services;

public interface IProcessingJobService
{
    Task<ProcessingJobResponse> CreateProcessingJobAsync(
        Guid documentId,
        CreateProcessingJobRequest request, CancellationToken cancellationToken);

    Task<ProcessingJobResponse?> GetProcessingJobByIdAsync(Guid jobId);

    Task<IReadOnlyList<ProcessingJobResponse>> GetProcessingJobsByDocumentIdAsync(Guid documentId);
}