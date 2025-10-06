using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core.DTO;

public class RegistryDTO
{
    [Required]
    public string IssuerId { get; init; } = string.Empty;

    [Required]
    public string PublicKey { get; init; } = string.Empty;

    [Required]
    public string Status { get; init; }

    public RegistryDTO(string issuerId, string publicKey, string status)
    {
        IssuerId = issuerId;
        PublicKey = publicKey;
        Status = status;
    }

}
