using DocumentIntelligence.Domain.Enums;

namespace DocumentIntelligence.Application.DTOs;

public class TenantResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ExternalReferenceId { get; set; } = string.Empty;

    public TenantStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}