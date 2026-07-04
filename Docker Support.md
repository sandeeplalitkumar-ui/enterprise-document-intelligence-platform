## Docker Support

The Document Intelligence API can be run inside a Docker container.

This project currently uses a **self-contained .NET publish** approach. The API is first published locally for Linux, and then Docker packages the published output into a lightweight Debian-based container image.

This approach was used as a practical fallback because Microsoft Container Registry image pulls were failing from Docker Desktop in the local environment.

---

### Current Docker Approach

```text
.NET source code
   ↓
dotnet publish
   ↓
publish folder
   ↓
docker build
   ↓
Docker image
   ↓
docker run
   ↓
API running inside Linux container
```

---

### 1. Publish the API for Linux

From the API project folder:

```bash
cd EnterpriseDocumentIntelligence/backend/DocumentIntelligence.Api
```

Run:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish
```

This creates a `publish` folder containing the API, required DLLs, and the .NET runtime files needed to run the application inside a Linux container.

---

### 2. Build the Docker Image

Run:

```bash
docker build -t document-intelligence-api .
```

This creates a Docker image named:

```text
document-intelligence-api
```

The image contains:

```text
- A lightweight Debian Linux base
- The published .NET API files
- Environment variables for ASP.NET Core
- A startup command to run the API
```

---

### 3. Run the API Container

Run the container in the background:

```bash
docker run -d --name document-api -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development document-intelligence-api
```

Open Swagger:

```text
http://localhost:8080/swagger
```

---

### 4. Useful Docker Commands

List running containers:

```bash
docker ps
```

List all containers, including stopped ones:

```bash
docker ps -a
```

View API container logs:

```bash
docker logs document-api
```

Stop the container:

```bash
docker stop document-api
```

Start the stopped container again:

```bash
docker start document-api
```

Remove the container:

```bash
docker rm document-api
```

List Docker images:

```bash
docker images
```

---

### 5. Image vs Container

A Docker image is the packaged application.

A Docker container is a running instance of that image.

```text
Docker image      = packaged app
Docker container  = running app
```

In this project:

```text
document-intelligence-api image
        ↓
document-api container
```

---

### 6. Current Dockerfile

```dockerfile
FROM debian:bookworm-slim

WORKDIR /app

COPY ./publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

ENTRYPOINT ["./DocumentIntelligence.Api"]
```

---

### 7. Dockerfile Explanation

```dockerfile
FROM debian:bookworm-slim
```

Uses a lightweight Debian Linux base image.

```dockerfile
WORKDIR /app
```

Creates and switches to the `/app` folder inside the container.

```dockerfile
COPY ./publish .
```

Copies the locally published .NET API files into the container.

```dockerfile
EXPOSE 8080
```

Documents that the containerized API listens on port `8080`.

```dockerfile
ENV ASPNETCORE_URLS=http://+:8080
```

Configures ASP.NET Core to listen on port `8080` inside the container.

```dockerfile
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
```

Uses invariant globalization mode for the minimal Linux container environment.

```dockerfile
ENTRYPOINT ["./DocumentIntelligence.Api"]
```

Starts the published self-contained .NET API executable when the container runs.

---

### 8. Notes

This Docker setup is intended for local learning and development.

In a production setup, a standard multi-stage Dockerfile would typically be used. That approach builds the application inside a .NET SDK container and runs it using the official ASP.NET runtime image.

Example production-style approach:

```text
.NET SDK image
   ↓
restore, build, publish
   ↓
ASP.NET runtime image
   ↓
run published API
```

The current self-contained approach avoids the need to pull Microsoft .NET base images during Docker build, which helped bypass local registry connectivity issues.
