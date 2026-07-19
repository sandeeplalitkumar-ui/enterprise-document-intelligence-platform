namespace DocumentIntelligence.Infrastructure.Services.TextExtraction;

public class PythonTextExtractionRequest
{
    public string DocumentId { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string StoragePath { get; set; } = string.Empty;
}

public class PythonTextExtractionResponse
{
    public string DocumentId { get; set; } = string.Empty;

    public string ExtractedText { get; set; } = string.Empty;
}