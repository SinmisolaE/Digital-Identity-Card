# TrustRegistrySolution

Trust Registry service (store issuer IDs + public keys).

## Overview
The Trust Registry stores issuer registrations (issuer id, public key, status) and exposes two simple REST endpoints used by Issuer and Verifier services.

- API entrypoint: [`TrustRegistryService.API.Program`](src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs](src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs)
- Controller: [`TrustRegistryService.API.Controllers.RegistryController`](src/backend/TrustRegistrySolution/TrustRegistryService.API/Controllers/RegistryController.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.API/Controllers/RegistryController.cs](src/backend/TrustRegistrySolution/TrustRegistryService.API/Controllers/RegistryController.cs)

## Projects & Key symbols
- Core DTO: [`TrustRegistryService.Core.DTO.RegistryDTO`](src/backend/TrustRegistrySolution/TrustRegistryService.Core/DTO/RegistryDTO.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.Core/DTO/RegistryDTO.cs](src/backend/TrustRegistrySolution/TrustRegistryService.Core/DTO/RegistryDTO.cs)  
- Core entity: [`TrustRegistryService.Core.Entity.TrustRegistry`](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Entity/TrustRegistry.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.Core/Entity/TrustRegistry.cs](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Entity/TrustRegistry.cs)  
- Core service: [`TrustRegistryService.Core.Services.RegistryService`](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Services/RegistryService.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.Core/Services/RegistryService.cs](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Services/RegistryService.cs)  
- Interfaces:  
  - [`TrustRegistryService.Core.Interfaces.IRegistryRepository`](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Interfaces/IRegistryRepository.cs) — [src/backend/TrustRegistrySolution/TrustRegistryService.Core/Interfaces/IRegistryRepository.cs](src/backend/TrustRegistrySolution/TrustRegistryService.Core/Interfaces/IRegistryRepository.cs)  
  - [`TrustRegistryService.Core.Interfaces.IRegistryService`](src/backend/TrustRegistrySolution/TrustRegistryService.Core