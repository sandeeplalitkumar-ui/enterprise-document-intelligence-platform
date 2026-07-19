using DocumentIntelligence.Api.Endpoints;
using DocumentIntelligence.Api.Workers;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Application.Queues;
using DocumentIntelligence.Application.Services;
using DocumentIntelligence.Infrastructure.Queues;
using DocumentIntelligence.Infrastructure.Repositories;
using DocumentIntelligence.Infrastructure.Services;
using DocumentIntelligence.Infrastructure.Services.TextExtraction;
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
builder.Services.AddSingleton<IDocumentTextExtractionRepository, InMemoryDocumentTextExtractionRepository>();
builder.Services.AddHttpClient<ITextExtractionService, HttpPythonTextExtractionService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var baseUrl = configuration["PythonTextExtractionService:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException("Python text extraction service BaseUrl is not configured.");
    }

    client.BaseAddress = new Uri(baseUrl);
});
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