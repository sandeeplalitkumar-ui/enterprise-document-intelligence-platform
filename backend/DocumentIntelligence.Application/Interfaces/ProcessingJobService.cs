using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;
using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.Services;

public class ProcessingJobService : IProcessingJobService
{
    private readonly IProcessingJobRepository _processingJobRepository;
    private readonly IDocumentRepository _documentRepository;

    public ProcessingJobService(
        IProcessingJobRepository processingJobRepository,
        IDocumentRepository documentRepository)
    {
        _processingJobRepository = processingJobRepository;
        _documentRepository = documentRepository;
    }

    public async Task<ProcessingJobResponse> CreateProcessingJobAsync(
        Guid documentId,
        CreateProcessingJobRequest request)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);

        if (document is null)
        {
            throw new ArgumentException($"Document with id {documentId} was not found.");
        }

        if (string.IsNullOrWhiteSpace(request.RequestedBy))
        {
            throw new ArgumentException("RequestedBy is required.");
        }

        var job = new ProcessingJob
        {
            Id = Guid.NewGuid(),
            TenantId = document.TenantId,
            DocumentId = document.Id,
            Status = ProcessingJobStatus.Pending,
            RetryCount = 0,
            CreatedAtUtc = DateTime.UtcNow,
            RequestedBy = request.RequestedBy.Trim(),
        };

        var createdJob = await _processingJobRepository.AddAsync(job);

        return MapToResponse(createdJob);
    }

    public async Task<ProcessingJobResponse?> GetProcessingJobByIdAsync(Guid jobId)
    {
        var job = await _processingJobRepository.GetByIdAsync(jobId);

        if (job is null)
        {
            return null;
        }

        return MapToResponse(job);
    }

    public async Task<IReadOnlyList<ProcessingJobResponse>> GetProcessingJobsByDocumentIdAsync(Guid documentId)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);

        if (document is null)
        {
            throw new ArgumentException($"Document with id {documentId} was not found.");
        }

        var jobs = await _processingJobRepository.GetByDocumentIdAsync(documentId);

        return jobs
            .Select(MapToResponse)
            .ToList();
    }

    private static ProcessingJobResponse MapToResponse(ProcessingJob job)
    {
        return new ProcessingJobResponse
        {
            Id = job.Id,
            TenantId = job.TenantId,
            DocumentId = job.DocumentId,
            Status = job.Status,
            RetryCount = job.RetryCount,
            FailureReason = job.FailureReason,
            CreatedAtUtc = job.CreatedAtUtc,
            StartedAtUtc = job.StartedAtUtc,
            CompletedAtUtc = job.CompletedAtUtc,
            RequestedBy = job.RequestedBy,
        };
    }
}