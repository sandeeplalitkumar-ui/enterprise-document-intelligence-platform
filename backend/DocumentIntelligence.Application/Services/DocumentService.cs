using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;
using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IFileStorageService _fileStorageService;

    public DocumentService(
        IDocumentRepository documentRepository,
        ITenantRepository tenantRepository,
        IFileStorageService fileStorageService)
    {
        _documentRepository = documentRepository;
        _tenantRepository = tenantRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<DocumentResponse> CreateDocumentAsync(Guid tenantId, CreateDocumentRequest request)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (tenant is null)
        {
            throw new ArgumentException($"Tenant with id {tenantId} was not found.");
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            throw new ArgumentException("File name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            throw new ArgumentException("Content type is required.");
        }

        if (request.SizeInBytes <= 0)
        {
            throw new ArgumentException("File size must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.StoragePath))
        {
            throw new ArgumentException("Storage path is required.");
        }

        var document = new Document
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            FileName = request.FileName.Trim(),
            ContentType = request.ContentType.Trim(),
            SizeInBytes = request.SizeInBytes,
            StoragePath = request.StoragePath.Trim(),
            Classification = request.Classification,
            Status = DocumentStatus.Uploaded,
            UploadedAtUtc = DateTime.UtcNow
        };

        var createdDocument = await _documentRepository.AddAsync(document);

        return MapToResponse(createdDocument);
    }

    public async Task<DocumentResponse> UploadDocumentAsync(Guid tenantId, UploadDocumentRequest request)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (tenant is null)
        {
            throw new ArgumentException($"Tenant with id {tenantId} was not found.");
        }

        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            throw new ArgumentException("File name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ContentType))
        {
            throw new ArgumentException("Content type is required.");
        }

        if (request.SizeInBytes <= 0)
        {
            throw new ArgumentException("File size must be greater than zero.");
        }

        const long maxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        if (request.SizeInBytes > maxFileSizeBytes)
        {
            throw new ArgumentException("File size cannot exceed 10 MB.");
        }

        var allowedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "text/plain",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        if (!allowedContentTypes.Contains(request.ContentType))
        {
            throw new ArgumentException("Unsupported file type. Allowed types are PDF, TXT, and DOCX.");
        }

        var storagePath = await _fileStorageService.SaveAsync(
            request.FileStream,
            request.FileName,
            request.ContentType,
            tenantId);

        var document = new Document
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            FileName = request.FileName.Trim(),
            ContentType = request.ContentType.Trim(),
            SizeInBytes = request.SizeInBytes,
            StoragePath = storagePath,
            Classification = request.Classification,
            Status = DocumentStatus.Uploaded,
            UploadedAtUtc = DateTime.UtcNow
        };

        var createdDocument = await _documentRepository.AddAsync(document);

        return MapToResponse(createdDocument);
    }

    public async Task<IReadOnlyList<DocumentResponse>> GetDocumentsByTenantAsync(Guid tenantId)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (tenant is null)
        {
            throw new ArgumentException($"Tenant with id {tenantId} was not found.");
        }

        var documents = await _documentRepository.GetByTenantIdAsync(tenantId);

        return documents
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<DocumentResponse?> GetDocumentByIdAsync(Guid documentId)
    {
        var document = await _documentRepository.GetByIdAsync(documentId);

        if (document is null)
        {
            return null;
        }

        return MapToResponse(document);
    }

    private static DocumentResponse MapToResponse(Document document)
    {
        return new DocumentResponse
        {
            Id = document.Id,
            TenantId = document.TenantId,
            FileName = document.FileName,
            ContentType = document.ContentType,
            SizeInBytes = document.SizeInBytes,
            StoragePath = document.StoragePath,
            Classification = document.Classification,
            Status = document.Status,
            UploadedAtUtc = document.UploadedAtUtc,
            ProcessedAtUtc = document.ProcessedAtUtc
        };
    }
}