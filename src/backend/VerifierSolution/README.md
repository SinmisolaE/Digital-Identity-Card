# VerifierSolution

Verifier service that validates citizen credentials (JWTs), issues nonce challenges, and exposes endpoints used by the Verifier frontend.

Contents
- Verifier API: [src/backend/VerifierSolution/Verifier.API/Program.cs](src/backend/VerifierSolution/Verifier.API/Program.cs)  
- Controller: [`Verifier.API.Controllers.VerifierController`](src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs)  
- Nonce logic: [`Verifier.Infrastructure.NonceService`](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs)  
- JWT verification: [`Verifier.Infrastructure.JwtVerifier`](src/backend/VerifierSolution/Verifier.Infrastructure/JwtVerifier.cs)  
- Core verification service: [`Verifier.Core.Services.VerifierService`](src/backend/VerifierSolution/Verifier.Core/Services/VerifierService.cs)  
- Interfaces:  
  - [`Verifier.Core.Interfaces.INonceService`](src/backend/VerifierSolution/Verifier.Core/Interfaces/INonceService.cs)  
  - [`Verifier.Core.Interfaces.IVerifierService`](src/backend/VerifierSolution/Verifier.Core/Interfaces/IVerifierService.cs)  
  - [`Verifier.Core.Interfaces.IJwtVerifier`](src/backend/VerifierSolution/Verifier.Core/Interfaces/IJwtVerifier.cs)

Why this project
- Provides nonce-challenge based verification flow for a verifier web app (QR + polling).
- Verifies JWT signature and extracts citizen information.
- Caches nonce states (generated / verified) and supports Redis-backed cache configuration.

Quick architecture summary
- HTTP endpoints are implemented in [`Verifier.API.Controllers.VerifierController`](src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs).
  - GET /challenge — generate nonce and return a verify URL (used by verifier frontend).
  - POST /verify — accept { nonce, Jwt } payload and run verification flow.
  - GET /status?nonce=... — return verification status / citizen data for a nonce.
- Nonce management is implemented in [`Verifier.Infrastructure.NonceService`](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs) and exposed via [`Verifier.Core.Interfaces.INonceService`](src/backend/VerifierSolution/Verifier.Core/Interfaces/INonceService.cs).
- JWT parsing & signature checking is implemented in [`Verifier.Infrastructure.JwtVerifier`](src/backend/VerifierSolution/Verifier.Infrastructure/JwtVerifier.cs) and consumed by [`Verifier.Core.Services.VerifierService`](src/backend/VerifierSolution/Verifier.Core/Services/Verifier