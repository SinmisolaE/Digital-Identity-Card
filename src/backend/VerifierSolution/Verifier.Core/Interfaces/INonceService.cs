using System;

namespace Verifier.Core.Interfaces;

public interface INonceService
{
    string GenerateNonce();

    bool IsValid(string nonce);

}
