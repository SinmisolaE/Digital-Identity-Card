# Issuer Solution

Short description
- The Issuer solution is responsible for creating signed JWT credentials for citizens and publishing issuer keys/metadata to the Trust Registry.

Quick links (core files & symbols)
- Entrypoint: [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs)
- Controller: [`Issuer.API.Controllers.IssuerController`](src/backend/IssuerSolution/Issuer.API/Controllers/IssuerController.cs)
- Service interface: [`Issuer.Core.Interfaces.IIssuerService`](src/backend/IssuerSolution/Issuer.Core/Interfaces/IIssuerService.cs)
- Service implementation: [`Issuer.Core.Service.IssuerService`](src/backend/IssuerSolution/Issuer.Core/Service/IssuerService.cs)
- JWT generator interface: [`Issuer.Core.Interfaces.IJwtGenerator`](src/backend/IssuerSolution/Issuer.Core/Interfaces/IJwtGenerator.cs)
- JWT generator implementation: [`Issuer.Infrastructure.JwtGenerator`](src/backend/IssuerSolution/Issuer.Infrastructure/Security/JwtGenerator.cs)
- RSA / key handling: [`Issuer.Infrastructure.RsaKeyService`](src/backend/IssuerSolution/Issuer.Infrastructure/Security/RsaKeyService.cs)
- Token provider: [`Issuer.Infrastructure.UserTokenProvider`](src/backend/IssuerSolution/Issuer.Infrastructure/Security/UserTokenProvider.cs)
- Trust registry client: [`Issuer.Infrastructure.TrustRegistryClient`](src/backend/IssuerSolution/Issuer.Infrastructure/TrustRegistryClient.cs)
- Data model: [`Issuer.Core.Data.Citizen`](src/backend/IssuerSolution/Issuer.Core/Data/Citizen.cs)
- DTO: [`Issuer.Core.DTO.CitizenDTO`](src/backend/IssuerSolution/Issuer.Core/DTO/CitizenDTO.cs)
- Registry DTO: [`Issuer.Core.DTO.RegistryDTO`](src/backend/IssuerSolution/Issuer.Core/DTO/RegistryDTO.cs)
- EF migrations (initial): [src/backend/IssuerSolution/Issuer.Infrastructure/Migrations/20251116033156_InitialCreate.cs](src/backend/IssuerSolution/Issuer.Infrastructure/Migrations/20251116033156_InitialCreate.cs)
- Configuration: [src/backend/IssuerSolution/Issuer.API/appsettings.json](src/backend/IssuerSolution/Issuer.API/appsettings.json)
- Logs folder (runtime logs): [src/backend/IssuerSolution/Issuer.API/Log/](src/backend/IssuerSolution/Issuer.API/Log/)

How it fits in the system
- Issues signed JWT credentials for citizens using RSA keys and the [`Issuer.Infrastructure.JwtGenerator`](src/backend/IssuerSolution/Issuer.Infrastructure/Security/JwtGenerator.cs).
- Publishes / communicates issuer public key and status (via `TrustRegistryClient`) to the Trust Registry service.
- Exposes REST endpoints in [`Issuer.API.Controllers.IssuerController`](src/backend/IssuerSolution/Issuer.API/Controllers/IssuerController.cs) used by frontends.

Run locally (developer)
1. Configure DB connection string in [src/backend/IssuerSolution/Issuer.API/appsettings.json](src/backend/IssuerSolution/Issuer.API/appsettings.json) under "ConnectionStrings:DefaultConnection".
2. From the Issuer API folder:
   - cd src/backend/IssuerSolution/Issuer.API
   - dotnet run
3. Migrations (if needed):
   - Ensure dotnet-ef tools are installed
   - Run migrations against the database with the migrations assembly:
     - dotnet ef database update --project ../Issuer.Infrastructure --startup-project . --context UserDbContext

Notes / tips
- The Program.cs registers services (see [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs)) including:
  - [`Issuer.Core.Interfaces.IIssuerService`](src/backend/IssuerSolution/Issuer.Core/Interfaces/IIssuerService.cs)
  - [`Issuer.Core.Interfaces.IJwtGenerator`](src/backend/IssuerSolution/Issuer.Core/Interfaces/IJwtGenerator.cs)
  - [`Issuer.Infrastructure.IRsaKeyService` — implemented by `Issuer.Infrastructure.RsaKeyService`](src/backend/IssuerSolution/Issuer.Infrastructure/Security/RsaKeyService.cs)
- Logs are written to `Log/log.txt` by default (see Program.cs).
- Check [`Issuer.API.Controllers.IssuerController`](src/backend/IssuerSolution/Issuer.API/Controllers/IssuerController.cs) for exposed endpoints and example routes.

Relevant workspace references
- Project entry/assemblies and build artifacts are in the usual bin/obj folders under each project (see obj lists in the workspace).
- Keep generated files (appsettings, migrations, compiled assemblies) out of source control if you modify them; see `keep.sh` for restore hints.