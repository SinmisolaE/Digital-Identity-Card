# Digital Identity Card

A microservice-based + frontend system for issuing, registering and verifying digital identity credentials.

This README gives a concise overview, quick start steps, layout of services, and troubleshooting pointers so contributors and maintainers can get productive quickly.

---

## Contents / Architecture

- Backend
  - Verifier service: verifies credentials and issues nonce challenges  
    - Implementation: [`Verifier.API.Controllers.VerifierController`](src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs) — see [src/backend/VerifierSolution/Verifier.API/Program.cs](src/backend/VerifierSolution/Verifier.API/Program.cs)
    - Nonce logic: [`Verifier.Infrastructure.NonceService`](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs)
  - Issuer service: creates credentials (JWTs) and publishes public keys to the registry  
    - Entrypoint: [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs)
  - Trust Registry service: stores issuer registration & public keys  
    - Entrypoint: [src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs](src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs)
  - Gateway (reverse proxy): routes frontend requests to backend services  
    - Entrypoint: [src/backend/Gateway/ApiGateway/Program.cs](src/backend/Gateway/ApiGateway/Program.cs)

- Frontend
  - Verifier web app (React): QR generation & polling — component: [`Verify`](frontend/verifier/app/src/components/Verify.js) — see [frontend/verifier/app/package.json](frontend/verifier/app/package.json)
  - Issuer web app (React): issue credentials — page: [`main`](frontend/issuer/app/src/pages/main.js) — see [frontend/issuer/app/package.json](frontend/issuer/app/package.json)
  - Citizen web app (React): receive & share credentials — pages: [`ReceiveCredential`](frontend/citizen/citizen-app/src/pages/receiveCredential.js) and [`Verify`](frontend/citizen/citizen-app/src/pages/Verify.js) — see [frontend/citizen/citizen-app/package.json](frontend/citizen/citizen-app/package.json)

Logs for services are written under each API's `Log/` folder (examples: [src/backend/VerifierSolution/Verifier.API/Log/](src/backend/VerifierSolution/Verifier.API/Log/) and [src/backend/IssuerSolution/Issuer.API/Log/](src/backend/IssuerSolution/Issuer.API/Log/)).

---

## Quick start (developer)

Prerequisites:
- .NET 9 SDK (dotnet)
- Node.js + npm
- Optional: Postman / curl for API testing

1. Backend — run APIs
   - Open a terminal per service and run from the service project folder:
     - Verifier API
       - Folder: `src/backend/VerifierSolution/Verifier.API`
       - See: [src/backend/VerifierSolution/Verifier.API/Program.cs](src/backend/VerifierSolution/Verifier.API/Program.cs)
       - Example:
         - cd src/backend/VerifierSolution/Verifier.API
         - dotnet run
     - Issuer API
       - Folder: `src/backend/IssuerSolution/Issuer.API`
       - See: [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs)
       - dotnet run
     - Trust Registry API
       - Folder: `src/backend/TrustRegistrySolution/TrustRegistryService.API`
       - See: [src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs](src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs)
       - dotnet run
     - Gateway (optional)
       - Folder: `src/backend/Gateway/ApiGateway`
       - See: [src/backend/Gateway/ApiGateway/Program.cs](src/backend/Gateway/ApiGateway/Program.cs)
       - dotnet run

   Ports used by services (observed in logs):
   - Verifier.API → http://localhost:5101 ([log reference](src/backend/VerifierSolution/Verifier.API/Log/log20251014.txt))
   - Issuer.API → http://localhost:5142 ([log reference](src/backend/IssuerSolution/Issuer.API/Log/log.txt))
   - Trust Registry → http://localhost:5051 (used by other services — see logs)

2. Frontend — start the React apps
   - Verifier app
     - Folder: `frontend/verifier/app`
     - npm install && npm start
     - Core component: [`Verify`](frontend/verifier/app/src/components/Verify.js)
   - Issuer app
     - Folder: `frontend/issuer/app`
     - npm install && npm start
     - Main page: [`main`](frontend/issuer/app/src/pages/main.js)
   - Citizen app
     - Folder: `frontend/citizen/citizen-app`
     - npm install && npm start
     - Pages: [`receiveCredential`](frontend/citizen/citizen-app/src/pages/receiveCredential.js), [`Verify`](frontend/citizen/citizen-app/src/pages/Verify.js)

3. Typical flow
   - Start Trust Registry, Issuer, Verifier (and Gateway if used).
   - Use Issuer UI to create a credential (Issuer issues a JWT signed with issuer keys).
   - Citizen app `ReceiveCredential` scans the Issuer QR and stores the credential (public key / JWT).
   - Verifier app generates a nonce challenge (via Verifier endpoint) and shows QR code (see [`Verify`](frontend/verifier/app/src/components/Verify.js)).
   - Citizen `Verify` scans verifier QR and posts stored credential + nonce to the Verifier API for verification.

Key implementation points:
- Nonce generation and caching: [`Verifier.Infrastructure.NonceService`](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs)
- Verification controller: `Verifier.API.Controllers.VerifierController` ([src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs](src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs))
- Issuing JWTs and key handling in Issuer solution: see [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs) and Issuer logs.

---

## Testing & troubleshooting

- Check service logs: each API writes to `Log/log.txt` or daily logs (see `src/backend/*/*/Log/`).
  - Example Verifier log: [src/backend/VerifierSolution/Verifier.API/Log/log20251014.txt](src/backend/VerifierSolution/Verifier.API/Log/log20251014.txt)
  - Example Issuer log: [src/backend/IssuerSolution/Issuer.API/Log/log.txt](src/backend/IssuerSolution/Issuer.API/Log/log.txt)
- Common issues
  - CORS: gateway config controls allowed origins (see [src/backend/Gateway/ApiGateway/Program.cs](src/backend/Gateway/ApiGateway/Program.cs)).
  - Port mismatches: frontends call specific ports (see [`Verify`](frontend/verifier/app/src/components/Verify.js) where the challenge is requested). Update endpoints if you change service ports.
  - Nonce expiration: nonces are stored in memory with a default expiry in [`Verifier.Infrastructure.NonceService`](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs).

---

## Contributing

- Follow the existing structure: each service lives under `src/backend/*` and each frontend under `frontend/*`.
- Run and test services locally via `dotnet run` (APIs) and `npm start` (frontends).
- Add unit / integration tests inside each project where appropriate.

---

## Useful files (quick links)

- Verifier API: [src/backend/VerifierSolution/Verifier.API/Program.cs](src/backend/VerifierSolution/Verifier.API/Program.cs)  
- Verifier controller: [src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs](src/backend/VerifierSolution/Verifier.API/Controllers/VerifierController.cs)  
- Nonce service: [src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs](src/backend/VerifierSolution/Verifier.Infrastructure/NonceService.cs)  
- Issuer API program: [src/backend/IssuerSolution/Issuer.API/Program.cs](src/backend/IssuerSolution/Issuer.API/Program.cs)  
- Trust Registry API program: [src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs](src/backend/TrustRegistrySolution/TrustRegistryService.API/Program.cs)  
- Gateway program: [src/backend/Gateway/ApiGateway/Program.cs](src/backend/Gateway/ApiGateway/Program.cs)  
- Verifier frontend component: [frontend/verifier/app/src/components/Verify.js](frontend/verifier/app/src/components/Verify.js)  
- Citizen receive page: [frontend/citizen/citizen-app/src/pages/receiveCredential.js](frontend/citizen/citizen-app/src/pages/receiveCredential.js)  
- Citizen verify page: [frontend/citizen/citizen-app/src/pages/Verify.js](frontend/citizen/citizen-app/src/pages/Verify.js)  
- Issuer main page: [frontend/issuer/app/src/pages/main.js](frontend/issuer/app/src/pages/main.js)

---

If you want, I can:
- generate this README file in the repo (apply the content above),
- or produce smaller per-service READMEs (backend/frontends) with step-by-step run scripts.
