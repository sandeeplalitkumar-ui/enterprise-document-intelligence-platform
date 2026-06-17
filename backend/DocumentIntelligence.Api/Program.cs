using DocumentIntelligence.Api.Endpoints;
using DocumentIntelligence.Api.Workers;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Application.Queues;
using DocumentIntelligence.Application.Services;
using DocumentIntelligence.Infrastructure.Queues;
using DocumentIntelligence.Infrastructure.Repositories;
using DocumentIntelligence.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITenantRepository, InMemoryTenantRepository>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddSingleton<IProcessingJobRepository, InMemoryProcessingJobRepository>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();
builder.Services.AddSingleton<IProcessingJobQueue, InMemoryProcessingJobQueue>();

builder.Services.AddHostedService<ProcessingJobWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        Status = "Healthy",
        Service = "Document Intelligence API",
        TimestampUtc = DateTime.UtcNow
    });
})
.WithName("HealthCheck")
.WithTags("Health");

app.MapTenantEndpoints();
app.MapDocumentEndpoints();
app.MapProcessingJobEndpoints();

app.Run();