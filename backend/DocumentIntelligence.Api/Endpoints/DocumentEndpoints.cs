using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Application.Services;
using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Api.Endpoints;

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(this WebApplication app)
    {

        app.MapGet("/api/documents/{documentId:guid}/text-extraction",
        async (
            Guid documentId,
            IDocumentTextExtractionRepository textExtractionRepository) =>
        {
            var extraction = await textExtractionRepository.GetByDocumentIdAsync(documentId);

            if (extraction is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(extraction);
        })
        .WithTags("Documents")
        .WithName("GetDocumentTextExtraction");

        app.MapPost("/api/tenants/{tenantId:guid}/documents", async (
            Guid tenantId,
            CreateDocumentRequest request,
            IDocumentService documentService) =>
        {
            try
            {
                var document = await documentService.CreateDocumentAsync(tenantId, request);

                return Results.Created($"/api/documents/{document.Id}", document);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("CreateDocument")
        .WithTags("Documents");

        app.MapPost("/api/tenants/{tenantId:guid}/documents/upload", async (
        Guid tenantId,
        IFormFile file,
        DocumentClassification classification,
        IDocumentService documentService) =>
        {
            try
            {
                if (file is null || file.Length == 0)
                {
                    return Results.BadRequest(new
                    {
                        Error = "File is required."
                    });
                }

                await using var stream = file.OpenReadStream();

                var request = new UploadDocumentRequest
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    SizeInBytes = file.Length,
                    FileStream = stream,
                    Classification = classification
                };

                var document = await documentService.UploadDocumentAsync(tenantId, request);

                return Results.Created($"/api/documents/{document.Id}", document);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("UploadDocument")
        .WithTags("Documents")
        .DisableAntiforgery();

        app.MapGet("/api/tenants/{tenantId:guid}/documents", async (
            Guid tenantId,
            IDocumentService documentService) =>
        {
            try
            {
                var documents = await documentService.GetDocumentsByTenantAsync(tenantId);

                return Results.Ok(documents);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("GetDocumentsByTenant")
        .WithTags("Documents");

        app.MapGet("/api/documents/{documentId:guid}", async (
            Guid documentId,
            IDocumentService documentService) =>
        {
            var document = await documentService.GetDocumentByIdAsync(documentId);

            if (document is null)
            {
                return Results.NotFound(new
                {
                    Error = $"Document with id {documentId} was not found."
                });
            }

            return Results.Ok(document);
        })
        .WithName("GetDocumentById")
        .WithTags("Documents");
    }
}