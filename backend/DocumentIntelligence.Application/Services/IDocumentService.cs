using DocumentIntelligence.Application.DTOs;

namespace DocumentIntelligence.Application.Services;

public interface IDocumentService
{
    Task<DocumentResponse> CreateDocumentAsync(Guid tenantId, CreateDocumentRequest request);

    Task<DocumentResponse> UploadDocumentAsync(Guid tenantId, UploadDocumentRequest request);

    Task<IReadOnlyList<DocumentResponse>> GetDocumentsByTenantAsync(Guid tenantId);

    Task<DocumentResponse?> GetDocumentByIdAsync(Guid documentId);
}