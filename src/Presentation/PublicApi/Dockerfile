# Syntax version
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80 443
# Add non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

# Package restore stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS restore
WORKDIR /tmp/build
# Copy only project files first
COPY ["src/Presentation/PublicApi/PublicApi.csproj", "src/Presentation/PublicApi/"]
COPY ["src/Infrastructure/External/External.csproj", "src/Infrastructure/External/"]
COPY ["src/Infrastructure/Migrator/Migrator.csproj", "src/Infrastructure/Migrator/"]
COPY ["src/Infrastructure/Persistence/Persistence.csproj", "src/Infrastructure/Persistence/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
# Restore packages
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "src/Presentation/PublicApi/PublicApi.csproj"

# Publish stage
FROM restore AS publish
ARG BUILD_CONFIGURATION=Release
COPY . .
WORKDIR "/tmp/build/src/Presentation/PublicApi"
# Publish with cached packages
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c ${BUILD_CONFIGURATION} -o /publish /p:UseAppHost=false

# Development stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS dev
COPY . ./source
WORKDIR "/source/src/Presentation/PublicApi"
CMD ["dotnet", "run", "--no-launch-profile"]

# Final stage
FROM base AS final
WORKDIR /app
# Copy published files
COPY --from=publish --chown=appuser:appgroup /publish .
# Health check
# HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
#     CMD wget --no-verbose --tries=1 --spider http://localhost:80/health || exit 1
# Set entry point
ENTRYPOINT ["dotnet", "PublicApi.dll"]