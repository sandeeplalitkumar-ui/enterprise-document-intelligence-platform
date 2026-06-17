using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DocumentIntelligence.Application.Queues;

namespace DocumentIntelligence.Infrastructure.Queues;

public class InMemoryProcessingJobQueue : IProcessingJobQueue
{
    private readonly Channel<Guid> _queue;

    public InMemoryProcessingJobQueue()
    {
        _queue = Channel.CreateUnbounded<Guid>();
    }

    public async ValueTask EnqueueAsync(Guid processingJobId, CancellationToken cancellationToken = default)
    {
        await _queue.Writer.WriteAsync(processingJobId, cancellationToken);
    }

    public async ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}