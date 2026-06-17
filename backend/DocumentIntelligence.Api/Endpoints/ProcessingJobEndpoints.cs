using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Services;

namespace DocumentIntelligence.Api.Endpoints;

public static class ProcessingJobEndpoints
{
    public static void MapProcessingJobEndpoints(this WebApplication app)
    {
        app.MapPost("/api/documents/{documentId:guid}/processing-jobs", async (
            Guid documentId,
            CreateProcessingJobRequest request,
            IProcessingJobService processingJobService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var job = await processingJobService.CreateProcessingJobAsync(documentId, request,cancellationToken);

                return Results.Created($"/api/processing-jobs/{job.Id}", job);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("CreateProcessingJob")
        .WithTags("Processing Jobs");

        app.MapGet("/api/processing-jobs/{jobId:guid}", async (
            Guid jobId,
            IProcessingJobService processingJobService) =>
        {
            var job = await processingJobService.GetProcessingJobByIdAsync(jobId);

            if (job is null)
            {
                return Results.NotFound(new
                {
                    Error = $"Processing job with id {jobId} was not found."
                });
            }

            return Results.Ok(job);
        })
        .WithName("GetProcessingJobById")
        .WithTags("Processing Jobs");

        app.MapGet("/api/documents/{documentId:guid}/processing-jobs", async (
            Guid documentId,
            IProcessingJobService processingJobService) =>
        {
            try
            {
                var jobs = await processingJobService.GetProcessingJobsByDocumentIdAsync(documentId);

                return Results.Ok(jobs);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("GetProcessingJobsByDocument")
        .WithTags("Processing Jobs");
    }
}