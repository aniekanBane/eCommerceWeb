FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
COPY . /source
WORKDIR "/source/src/Presentation/PublicApi"
RUN dotnet restore
RUN dotnet build -c $configuration -o /app/build --no-restore

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS dev
COPY . /source
WORKDIR "/source/src/Presentation/PublicApi"
CMD dotnet run --no-launch-profile

FROM build AS publish
ARG configuration=Release
RUN dotnet publish -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG UID=56785
RUN adduser \
    --uid $UID \
    --disabled-password \
    --gecos "" \
    appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "PublicApi.dll"]
