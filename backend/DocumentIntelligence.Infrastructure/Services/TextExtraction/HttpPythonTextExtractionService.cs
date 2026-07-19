using System.Net.Http.Json;
using DocumentIntelligence.Application.Interfaces;
using DocumentIntelligence.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace DocumentIntelligence.Infrastructure.Services.TextExtraction;

public class HttpPythonTextExtractionService : ITextExtractionService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpPythonTextExtractionService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> ExtractTextAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        var request = new PythonTextExtractionRequest
        {
            DocumentId = document.Id.ToString(),
            FileName = document.FileName,
            StoragePath = document.StoragePath
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/extract-text",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var extractionResponse =
            await response.Content.ReadFromJsonAsync<PythonTextExtractionResponse>(
                cancellationToken: cancellationToken);

        return extractionResponse?.ExtractedText ?? string.Empty;
    }
}