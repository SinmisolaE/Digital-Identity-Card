using System;

using Issuer.API.DTO;

namespace Issuer.Core.Interfaces.IService;

public interface IIssuerService
{
    string CreateCitizen(CitizenDTO citizenDTO);

    // would be used in giving citizen a new ID card, say it expires
    Task DeleteCitizen(CitizenDTO citizenDTO); //deletes the citizen national ID from db not implemented for now.


}
