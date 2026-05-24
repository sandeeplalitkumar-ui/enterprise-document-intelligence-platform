using DocumentIntelligence.Application.DTOs;
using DocumentIntelligence.Application.Services;

namespace DocumentIntelligence.Api.Endpoints;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tenants", async (
            CreateTenantRequest request,
            ITenantService tenantService) =>
        {
            try
            {
                var tenant = await tenantService.CreateTenantAsync(request);

                return Results.Created($"/api/tenants/{tenant.Id}", tenant);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    Error = ex.Message
                });
            }
        })
        .WithName("CreateTenant")
        .WithTags("Tenants");

        app.MapGet("/api/tenants", async (ITenantService tenantService) =>
        {
            var tenants = await tenantService.GetAllTenantsAsync();

            return Results.Ok(tenants);
        })
        .WithName("GetTenants")
        .WithTags("Tenants");

        app.MapGet("/api/tenants/{id:guid}", async (
            Guid id,
            ITenantService tenantService) =>
        {
            var tenant = await tenantService.GetTenantByIdAsync(id);

            if (tenant is null)
            {
                return Results.NotFound(new
                {
                    Error = $"Tenant with id {id} was not found."
                });
            }

            return Results.Ok(tenant);
        })
        .WithName("GetTenantById")
        .WithTags("Tenants");
    }
}