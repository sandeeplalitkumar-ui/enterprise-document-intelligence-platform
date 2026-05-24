using DocumentIntelligence.Application.DTOs;

namespace DocumentIntelligence.Application.Services;

public interface ITenantService
{
    Task<TenantResponse> CreateTenantAsync(CreateTenantRequest request);

    Task<IReadOnlyList<TenantResponse>> GetAllTenantsAsync();

    Task<TenantResponse?> GetTenantByIdAsync(Guid id);
}