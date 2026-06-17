using DocumentIntelligence.Application.Queues;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Api.Workers;

public class ProcessingJobWorker : BackgroundService
{
    private readonly IProcessingJobQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProcessingJobWorker> _logger;

    public ProcessingJobWorker(
        IProcessingJobQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<ProcessingJobWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Processing job worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var processingJobId = await _queue.DequeueAsync(stoppingToken);

                await ProcessJobAsync(processingJobId, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Processing job worker is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in processing job worker.");
            }
        }
    }

    private async Task ProcessJobAsync(Guid processingJobId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var processingJobRepository =
            scope.ServiceProvider.GetRequiredService<IProcessingJobRepository>();

        var documentRepository =
            scope.ServiceProvider.GetRequiredService<IDocumentRepository>();

        var job = await processingJobRepository.GetByIdAsync(processingJobId);

        if (job is null)
        {
            _logger.LogWarning("Processing job {ProcessingJobId} was not found.", processingJobId);
            return;
        }

        var document = await documentRepository.GetByIdAsync(job.DocumentId);

        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} was not found for processing job {ProcessingJobId}.",
                job.DocumentId,
                processingJobId);

            job.MarkAsFailed("Document not found.");
            await processingJobRepository.UpdateAsync(job);

            return;
        }

        _logger.LogInformation("Processing job {ProcessingJobId} started.", processingJobId);

        job.MarkAsProcessing();
        await processingJobRepository.UpdateAsync(job);

        // Simulate document processing for now.
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        document.MarkAsProcessed();
        await documentRepository.UpdateAsync(document);

        job.MarkAsSucceeded();
        await processingJobRepository.UpdateAsync(job);

        _logger.LogInformation("Processing job {ProcessingJobId} completed successfully.", processingJobId);
    }
}