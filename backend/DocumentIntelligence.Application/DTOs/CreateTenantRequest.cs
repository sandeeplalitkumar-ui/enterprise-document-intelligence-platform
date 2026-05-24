namespace DocumentIntelligence.Application.DTOs;

public class CreateTenantRequest
{
    public string Name { get; set; } = string.Empty;

    public string ExternalReferenceId { get; set; } = string.Empty;
}