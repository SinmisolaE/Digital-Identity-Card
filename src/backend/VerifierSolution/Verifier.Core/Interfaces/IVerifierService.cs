using System;
using Verifier.Core.DTO;

namespace Verifier.Core.Interfaces;

public interface IVerifierService
{
    Task<CitizenDTO> GetCitizenAsync(JwtDTO jwtDTO);

    string GetUrl();
}
