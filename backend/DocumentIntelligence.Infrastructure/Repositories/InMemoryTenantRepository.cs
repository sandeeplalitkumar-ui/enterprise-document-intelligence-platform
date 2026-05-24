using System.Collections.Concurrent;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;

namespace DocumentIntelligence.Infrastructure.Repositories;

public class InMemoryTenantRepository : ITenantRepository
{
    private readonly ConcurrentDictionary<Guid, Tenant> _tenants = new();

    public Task<Tenant> AddAsync(Tenant tenant)
    {
        _tenants[tenant.Id] = tenant;

        return Task.FromResult(tenant);
    }

    public Task<IReadOnlyList<Tenant>> GetAllAsync()
    {
        IReadOnlyList<Tenant> tenants = _tenants.Values
            .OrderBy(t => t.CreatedAtUtc)
            .ToList();

        return Task.FromResult(tenants);
    }

    public Task<Tenant?> GetByIdAsync(Guid id)
    {
        _tenants.TryGetValue(id, out var tenant);

        return Task.FromResult(tenant);
    }
}