# Restore base appsettings.json (this should stay!)
git checkout -- src/backend/IssuerSolution/Issuer.API/appsettings.json

# Restore source code files
git checkout -- src/backend/IssuerSolution/Issuer.Infrastructure/Email/EmailService.cs
git checkout -- src/backend/IssuerSolution/Issuer.Infrastructure/Issuer.Infrastructure.csproj
git checkout -- src/backend/IssuerSolution/Issuer.Infrastructure/Security/RsaKeyService.cs

# Restore .gitignore changes (if you made improvements)
git add src/backend/.gitignore