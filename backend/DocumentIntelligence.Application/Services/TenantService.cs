using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;
using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantResponse> CreateTenantAsync(CreateTenantRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Tenant name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ExternalReferenceId))
        {
            throw new ArgumentException("External reference id is required.");
        }

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ExternalReferenceId = request.ExternalReferenceId.Trim(),
            Status = TenantStatus.Active,
            CreatedAtUtc = DateTime.UtcNow
        };

        var createdTenant = await _tenantRepository.AddAsync(tenant);

        return MapToResponse(createdTenant);
    }

    public async Task<IReadOnlyList<TenantResponse>> GetAllTenantsAsync()
    {
        var tenants = await _tenantRepository.GetAllAsync();

        return tenants
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<TenantResponse?> GetTenantByIdAsync(Guid id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);

        if (tenant is null)
        {
            return null;
        }

        return MapToResponse(tenant);
    }

    private static TenantResponse MapToResponse(Tenant tenant)
    {
        return new TenantResponse
        {
            Id = tenant.Id,
            Name = tenant.Name,
            ExternalReferenceId = tenant.ExternalReferenceId,
            Status = tenant.Status,
            CreatedAtUtc = tenant.CreatedAtUtc
        };
    }
}