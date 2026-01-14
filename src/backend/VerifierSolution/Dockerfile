FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["Verifier.API/Verifier.API.csproj", "Verifier.API/"]
COPY ["Verifier.Core/Verifier.Core.csproj", "Verifier.Core/"]
COPY ["Verifier.Infrastructure/Verifier.Infrastructure.csproj", "Verifier.Infrastructure/"]

RUN dotnet restore "Verifier.API/Verifier.API.csproj"

COPY . .

RUN dotnet build "Verifier.API/Verifier.API.csproj" -c Release -o /app/build

RUN dotnet publish "Verifier.API/Verifier.API.csproj" -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

RUN adduser --disabled-password=true --home /app --gecos '' appUser && chown -R appUser:appUser /app
USER appUser

ENTRYPOINT ["dotnet", "Verifier.API.dll"]