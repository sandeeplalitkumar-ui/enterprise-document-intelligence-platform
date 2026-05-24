using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string ExternalReferenceId { get; set; } = string.Empty;

    public TenantStatus Status { get; set; } = TenantStatus.Pending;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public List<Document> Documents { get; set; } = new();
}