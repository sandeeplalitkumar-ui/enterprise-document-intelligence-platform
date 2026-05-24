using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Application.Interfaces;

public interface IProcessingJobRepository
{
    Task<ProcessingJob> AddAsync(ProcessingJob job);

    Task<ProcessingJob?> GetByIdAsync(Guid jobId);

    Task<IReadOnlyList<ProcessingJob>> GetByDocumentIdAsync(Guid documentId);

    Task UpdateAsync(ProcessingJob job);
}