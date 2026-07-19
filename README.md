# Enterprise Document Intelligence Platform

A multi-tenant, event-driven document intelligence platform built with **.NET 8**, designed to support document upload, metadata management, asynchronous document processing, and future AI-powered document search using RAG.

This project is part of my hands-on learning journey to strengthen skills in:

- Backend engineering
- Clean architecture
- System design
- Cloud-ready application design
- GenAI platform architecture
- Enterprise document workflows

---

## Project Overview

Enterprises often manage large volumes of documents across multiple customers, teams, departments, and access boundaries.

This project simulates a real-world enterprise backend platform where users can:

- Create tenants
- Upload documents
- Store document metadata
- Track document processing jobs
- Prepare documents for future AI-powered search and question answering

The long-term goal is to build a complete document intelligence system:

```text
Document Upload
→ Metadata Storage
→ Processing Job Creation
→ Queue-Based Async Processing
→ Text Extraction
→ Chunking
→ Embeddings
→ Vector Search
→ AI-Powered Q&A with Citations
```

---

## Why I Built This

This project is intentionally built step by step to practice production-style backend and system design concepts.

Instead of directly jumping into AI features, the foundation focuses on:

- Clean backend structure
- Proper separation of concerns
- Multi-tenant design
- File upload workflows
- Metadata vs file storage separation
- Processing job lifecycle
- Async processing readiness
- Future RAG pipeline integration

The goal is to build something closer to a real enterprise platform, not just a simple CRUD application.

---

## Current Milestone

The backend currently supports:

- Health check endpoint
- Tenant API
- Document metadata API
- File upload API
- Processing job API

Current flow:

```text
Create Tenant
→ Upload Document
→ Save File Locally
→ Store Document Metadata
→ Create Processing Job
→ Track Processing Job Status
```

Next milestone:

```text
Create Processing Job
→ Enqueue Job
→ Background Worker Picks Job
→ Mark Job as Processing
→ Simulate Processing
→ Mark Job as Succeeded
```

---

## Tech Stack

### Backend

- .NET 8
- ASP.NET Core Minimal APIs
- C#
- Dependency Injection
- Clean / Modular Architecture

### Current Storage

- In-memory repositories
- Local file storage

### Planned Storage

- SQL database for metadata
- Azure Blob Storage / AWS S3 / Google Cloud Storage for document files
- Vector database for embeddings

### Planned AI Components

- Text extraction
- Document chunking
- Embedding generation
- Vector search
- RAG query API
- AI-generated answers with source citations

---

## Repository Structure

```text
enterprise-document-intelligence-platform/
│
├── README.md
├── .gitignore
│
└── backend/
    ├── EnterpriseDocumentIntelligence.sln
    │
    ├── DocumentIntelligence.Api/
    │   ├── Endpoints/
    │   ├── Program.cs
    │   ├── appsettings.json
    │   └── DocumentIntelligence.Api.csproj
    │
    ├── DocumentIntelligence.Application/
    │   ├── DTOs/
    │   ├── Interfaces/
    │   ├── Services/
    │   └── DocumentIntelligence.Application.csproj
    │
    ├── DocumentIntelligence.Domain/
    │   ├── Entities/
    │   ├── Enums/
    │   └── DocumentIntelligence.Domain.csproj
    │
    └── DocumentIntelligence.Infrastructure/
        ├── Repositories/
        ├── Storage/
        └── DocumentIntelligence.Infrastructure.csproj
```

---

## Architecture

```text
Client / Swagger
        ↓
ASP.NET Core API
        ↓
Application Services
        ↓
Repository Interfaces
        ↓
Infrastructure Implementations
        ↓
In-Memory Store + Local File Storage
```

Future architecture:

```text
Client
  ↓
.NET API
  ↓
SQL Metadata Database
  ↓
Object Storage
  ↓
Processing Queue
  ↓
Background Worker
  ↓
Text Extraction
  ↓
Chunking
  ↓
Embedding Generation
  ↓
Vector Store
  ↓
RAG Query API
  ↓
AI Answer with Citations
```

---

## Layer Responsibilities

### API Layer

Responsible for:

- HTTP endpoints
- Request binding
- Response formatting
- Status codes
- Swagger support

### Application Layer

Responsible for:

- Use cases
- Business logic
- Service interfaces
- Repository contracts
- DTOs

### Domain Layer

Responsible for:

- Core entities
- Enums
- Business concepts

### Infrastructure Layer

Responsible for:

- Repository implementations
- File storage implementation
- Future database/cloud integrations

---

## Domain Entities

Current domain entities:

```text
Tenant
Document
DocumentChunk
ProcessingJob
AuditLog
```

Current enums:

```text
TenantStatus
DocumentStatus
ProcessingJobStatus
DocumentClassification
```

---

## API Endpoints

## Health

### Check API Health

```http
GET /health
```

---

## Tenant APIs

### Create Tenant

```http
POST /api/tenants
```

Sample request:

```json
{
  "name": "Contoso Finance",
  "externalReferenceId": "contoso-finance"
}
```

### Get All Tenants

```http
GET /api/tenants
```

### Get Tenant by ID

```http
GET /api/tenants/{id}
```

---

## Document Metadata APIs

### Create Document Metadata

```http
POST /api/tenants/{tenantId}/documents
```

Sample request:

```json
{
  "fileName": "sample.pdf",
  "contentType": "application/pdf",
  "sizeInBytes": 102400,
  "storagePath": "storage/documents/sample.pdf",
  "classification": 3
}
```

### Get Documents by Tenant

```http
GET /api/tenants/{tenantId}/documents
```

### Get Document by ID

```http
GET /api/documents/{documentId}
```

---

## File Upload API

### Upload Document

```http
POST /api/tenants/{tenantId}/documents/upload
```

Supported file types:

```text
PDF
TXT
DOCX
```

Maximum file size:

```text
10 MB
```

Current local storage path:

```text
storage/documents/{tenantId}/{generatedFileName}
```

Important design decision:

```text
Document metadata and document content are stored separately.

Metadata is structured data and belongs in a database.
Actual file content is unstructured binary data and belongs in object storage.
```

---

## Processing Job APIs

### Create Processing Job

```http
POST /api/documents/{documentId}/processing-jobs
```

Sample request:

```json
{
  "requestedBy": "system"
}
```

### Get Processing Job by ID

```http
GET /api/processing-jobs/{jobId}
```

### Get Processing Jobs by Document

```http
GET /api/documents/{documentId}/processing-jobs
```

Processing job statuses:

```text
Pending
Processing
Succeeded
Failed
DeadLettered
```

---

## Key Design Decisions

## 1. Modular Monolith First

This project starts as a modular monolith.

Reason:

```text
A clean modular monolith is easier to build, test, and evolve.
Microservices should be introduced only when there is a real need for
independent scaling, deployment, ownership, or fault isolation.
```

---

## 2. Metadata and File Content Are Separate

The platform stores metadata separately from file content.

Metadata includes:

```text
Document ID
Tenant ID
File name
Content type
Size
Classification
Status
Storage path
Upload timestamp
```

File content is stored separately in local storage for now.

Later, local storage can be replaced with:

```text
Azure Blob Storage
AWS S3
Google Cloud Storage
```

---

## 3. Long-Running Work Should Be Async

Document upload should not directly perform expensive processing.

Better design:

```text
Upload request completes quickly.
A processing job is created.
The job is handled asynchronously by a worker.
```

This improves:

- API responsiveness
- Reliability
- Scalability
- Retry handling
- Failure isolation

---

## 4. Application Layer Defines Contracts

Repository interfaces live in the Application layer.

Example:

```text
ITenantRepository
IDocumentRepository
IProcessingJobRepository
```

Infrastructure provides the concrete implementations.

This keeps business logic independent from storage details.

---

## How to Run Locally

### Prerequisites

Install:

- .NET 8 SDK
- Git
- Visual Studio Code, Rider, or Visual Studio

For Apple Silicon Macs, install the **ARM64 .NET SDK**.

---

### Clone Repository

```bash
git clone https://github.com/YOUR_USERNAME/enterprise-document-intelligence-platform.git
cd enterprise-document-intelligence-platform
```

---

### Restore Dependencies

```bash
dotnet restore
```

---

### Build Solution

```bash
dotnet build
```

---

### Run API

```bash
cd backend/DocumentIntelligence.Api
dotnet run
```

---

### Open Swagger

After running the API, open Swagger in the browser.

The API will show the local URL in the terminal, usually similar to:

```text
https://localhost:{port}/swagger
```

or

```text
http://localhost:{port}/swagger
```

---

## Example End-to-End Flow

### 1. Create Tenant

```http
POST /api/tenants
```

### 2. Upload Document

```http
POST /api/tenants/{tenantId}/documents/upload
```

### 3. Create Processing Job

```http
POST /api/documents/{documentId}/processing-jobs
```

### 4. Check Processing Job Status

```http
GET /api/processing-jobs/{jobId}
```

---

## Roadmap

## Roadmap

### Completed

- [x] Solution setup
- [x] Clean project structure
- [x] Domain entities
- [x] Health endpoint
- [x] Tenant API
- [x] Document metadata API
- [x] File upload API
- [x] Processing job API
- [x] In-memory processing queue
- [x] Background worker
- [x] Processing job status transitions
- [x] Simulated document processing
- [x] Document text extraction domain entity
- [x] In-memory text extraction repository
- [x] Background worker creates text extraction output
- [x] GET endpoint for document text extraction
- [x] Dockerized API using self-contained .NET publish
- [x] Docker support documentation
- [x] Python text extraction service
- [x] .NET to Python text extraction integration
- [x] HttpClient-based text extraction service

---

### Next

- [ ] Dockerize Python text extraction service
- [ ] Add Docker Compose for .NET API + Python service
- [ ] Add retry/error handling for Python service failure

---

### Planned

- [ ] Real PDF text extraction
- [ ] Document chunking
- [ ] Chunk metadata storage
- [ ] Embedding generation
- [ ] Vector search integration
- [ ] AI-powered Q&A over uploaded documents
- [ ] Citation support for answers
- [ ] Persistent database storage
- [ ] Replace in-memory queue with durable queue
- [ ] Authentication and authorization
- [ ] Tenant-level isolation and access control
- [ ] Cloud deployment

---

## Future Target Architecture

```text
Client
  ↓
API Gateway / .NET API
  ↓
Tenant + Document Services
  ↓
SQL Metadata Database
  ↓
Object Storage
  ↓
Queue
  ↓
Background Worker
  ↓
Text Extraction
  ↓
Chunking
  ↓
Embedding Service
  ↓
Vector Database
  ↓
RAG Query API
  ↓
LLM Answer with Citations
```

---

## Git Ignore Recommendations

The following should not be committed:

```gitignore
bin/
obj/
.vs/
.vscode/
*.user
.env
storage/documents/
**/storage/documents/
appsettings.Development.json
.DS_Store
```

---

## Learning Focus

This project helps me practice:

- Designing backend systems from scratch
- Applying clean architecture principles
- Building multi-tenant APIs
- Separating metadata from object storage
- Designing async processing workflows
- Preparing for cloud-native architecture
- Connecting backend engineering with GenAI systems

---

## Author

Built by **Sandeep Kumar Ganesan** as part of a hands-on senior engineering, system design, and GenAI platform learning journey.

---

## Disclaimer

This project is currently a learning and portfolio project. It is being built incrementally toward a production-style enterprise document intelligence platform.
