# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files first (for better caching)
COPY BespokedBikes.Domain/BespokedBikes.Domain.csproj BespokedBikes.Domain/
COPY BespokedBikes.Application/BespokedBikes.Application.csproj BespokedBikes.Application/
COPY BespokedBikes.Infrastructure/BespokedBikes.Infrastructure.csproj BespokedBikes.Infrastructure/
COPY BespokedBikes.Api/BespokedBikes.Api.csproj BespokedBikes.Api/

# Restore dependencies for API project (this will restore all project references)
RUN dotnet restore BespokedBikes.Api/BespokedBikes.Api.csproj

# Copy only the source code we need
COPY BespokedBikes.Domain/ BespokedBikes.Domain/
COPY BespokedBikes.Application/ BespokedBikes.Application/
COPY BespokedBikes.Infrastructure/ BespokedBikes.Infrastructure/
COPY BespokedBikes.Api/ BespokedBikes.Api/

# Build the application
WORKDIR /src/BespokedBikes.Api
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create a non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "BespokedBikes.Api.dll"]
