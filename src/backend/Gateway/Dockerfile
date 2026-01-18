FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ["Gateway.API/Gateway.API.csproj", "Gateway.API/"]

RUN dotnet restore "Gateway.API/Gateway.API.csproj"

COPY . .

RUN dotnet build "Gateway.API/Gateway.API.csproj" -c Release -o /app/build


RUN dotnet publish "Gateway.API/Gateway.API.csproj" -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .

RUN useradd -m -d /app appUser && chown -R appUser:appUser /app
USER appUser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Gateway.API.dll"]