# Syntax version
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
# Add non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

# Package Restore stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS restore
WORKDIR /tmp/build
# Copy only project files first
COPY ["src/Presentation/WebApp/WebApp.csproj", "src/Presentation/WebApp/"]
COPY ["src/Presentation/AdminPanel/AdminPanel.csproj", "src/Presentation/AdminPanel/"]
COPY ["src/Infrastructure/External/External.csproj", "src/Infrastructure/External/"]
COPY ["src/Infrastructure/Migrator/Migrator.csproj", "src/Infrastructure/Migrator/"]
COPY ["src/Infrastructure/Persistence/Persistence.csproj", "src/Infrastructure/Persistence/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
# Restore packages
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Presentation/AdminPanel"

# Publish stage
FROM restore AS publish
ARG BUILD_CONFIGURATION=Release
COPY . .
WORKDIR "/tmp/build/src/Presentation/WebApp"
# Publish project with cache packages
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c ${BUILD_CONFIGURATION} -o /publish /p:UseAppHost=false

# Development stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS dev
RUN apk update; apk add openssl
RUN mkdir -p /data-protection && openssl req -x509 -newkey rsa:4096 -sha256 -days 365 -nodes \
    -keyout /data-protection/private.key -out /data-protection/cert.pem \
    -subj "/C=US/ST=State/L=City/O=Organization/OU=Department/CN=localhost"
COPY . /source
WORKDIR "/source/src/Presentation/WebApp"
ENV ASPNETCORE_DATAPROTECTION_CERTIFICATE=/data-protection/cert.pem
CMD ["dotnet", "run", "--no-launch-profile"]

# Final stage
FROM base AS final
WORKDIR /app
# Copy published files
COPY --from=publish --chown=appuser:appgroup /publish .
# Set entry point
ENTRYPOINT ["dotnet", "WebApp.dll"]