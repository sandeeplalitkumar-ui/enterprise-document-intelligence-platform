using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentIntelligence.Application.Queues;

public interface IProcessingJobQueue
{
    ValueTask EnqueueAsync(Guid processingJobId, CancellationToken cancellationToken = default);

    ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken = default);
}