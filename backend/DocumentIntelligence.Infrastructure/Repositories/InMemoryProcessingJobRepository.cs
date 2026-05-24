using System.Collections.Concurrent;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Infrastructure.Repositories;

public class InMemoryProcessingJobRepository : IProcessingJobRepository
{
    private readonly ConcurrentDictionary<Guid, ProcessingJob> _jobs = new();

    public Task<ProcessingJob> AddAsync(ProcessingJob job)
    {
        _jobs[job.Id] = job;

        return Task.FromResult(job);
    }

    public Task<ProcessingJob?> GetByIdAsync(Guid jobId)
    {
        _jobs.TryGetValue(jobId, out var job);

        return Task.FromResult(job);
    }

    public Task<IReadOnlyList<ProcessingJob>> GetByDocumentIdAsync(Guid documentId)
    {
        IReadOnlyList<ProcessingJob> jobs = _jobs.Values
            .Where(j => j.DocumentId == documentId)
            .OrderByDescending(j => j.CreatedAtUtc)
            .ToList();

        return Task.FromResult(jobs);
    }

    public Task UpdateAsync(ProcessingJob job)
    {
        _jobs[job.Id] = job;

        return Task.CompletedTask;
    }
}