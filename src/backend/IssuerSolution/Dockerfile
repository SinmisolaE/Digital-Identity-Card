FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Issuer.API/Issuer.API.csproj", "Issuer.API/"]
COPY ["Issuer.Core/Issuer.Core.csproj", "Issuer.Core/"]
COPY ["Issuer.Infrastructure/Issuer.Infrastructure.csproj", "Issuer.Infrastructure/"]

RUN dotnet restore "Issuer.API/Issuer.API.csproj"

COPY . .

WORKDIR "/src/Issuer.API"
RUN dotnet build "Issuer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Issuer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080

RUN useradd -m -d /app appUser && chown -R appUser:appUser /app
USER appUser

ENTRYPOINT ["dotnet", "Issuer.API.dll"]
