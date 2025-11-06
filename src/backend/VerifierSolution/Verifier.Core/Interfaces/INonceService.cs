using System;
using Verifier.Core.DTO;

namespace Verifier.Core.Interfaces;

public interface INonceService
{
    string GenerateNonce();

    bool MarkAsVerified(string nonce, CitizenDTO citizenData);

    object GetStatus(string nonce);

    bool IsValid(string nonce);

}
