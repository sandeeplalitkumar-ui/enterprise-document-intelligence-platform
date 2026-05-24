using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Application.Interfaces;

public interface ITenantRepository
{
    Task<Tenant> AddAsync(Tenant tenant);

    Task<IReadOnlyList<Tenant>> GetAllAsync();

    Task<Tenant?> GetByIdAsync(Guid id);
}