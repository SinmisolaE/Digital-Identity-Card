FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["TrustRegistryService.API/TrustRegistryService.API.csproj", "TrustRegistryService.API/"]
COPY ["TrustRegistryService.Core/TrustRegistryService.Core.csproj", "TrustRegistryService.Core/"]
COPY ["TrustRegistryService.Infrastructure/TrustRegistryService.Infrastructure.csproj", "TrustRegistryService.Infrastructure/"]

RUN dotnet restore "TrustRegistryService.API.csproj"

COPY . .

RUN dotnet build "TrustRegistryService.API.csproj" -c Release -o /app/build

RUN dotnet publish "TrustRegistryService.API.csproj" -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .

RUN adduser --disabled-password=true --gecos '' --home /app appUser && chown -R appUser:appUser /app
USER appUser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "TrustRegistryService.API.dll"]