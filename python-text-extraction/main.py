from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI(title="Document Intelligence Text Extraction Service")

class TextExtractionRequest(BaseModel):
    documentId: str
    fileName: str
    storagePath: str

class TextExtractionResponse(BaseModel):
    documentId: str
    extractedText: str

@app.get("/health")
def health():
    return {
        "status": "healthy",
        "service": "python-text-extraction"
    }

@app.post("/extract-text", response_model=TextExtractionResponse)
def extract_text(request: TextExtractionRequest):
    extracted_text = f"Extracted text from Python service for document: {request.fileName}"

    return TextExtractionResponse(
        documentId=request.documentId,
        extractedText=extracted_text
    )