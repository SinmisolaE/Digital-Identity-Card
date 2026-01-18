FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ["TrustRegistryService.API/TrustRegistryService.API.csproj", "TrustRegistryService.API/"]
COPY ["TrustRegistryService.Core/TrustRegistryService.Core.csproj", "TrustRegistryService.Core/"]
COPY ["TrustRegistryService.Infrastructure/TrustRegistryService.Infrastructure.csproj", "TrustRegistryService.Infrastructure/"]

RUN dotnet restore "TrustRegistryService.API/TrustRegistryService.API.csproj"

COPY . .

RUN dotnet build "TrustRegistryService.API/TrustRegistryService.API.csproj" -c Release -o /app/build

RUN dotnet publish "TrustRegistryService.API/TrustRegistryService.API.csproj" -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .

RUN useradd -m -d /app appUser && chown -R appUser:appUser /app
USER appUser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "TrustRegistryService.API.dll"]