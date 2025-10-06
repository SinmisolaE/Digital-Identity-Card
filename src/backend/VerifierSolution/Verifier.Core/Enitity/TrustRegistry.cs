using System;
using System.ComponentModel.DataAnnotations;

namespace Verifier.Core.Entity;

public class TrustRegistry
{
    public enum SetStatus {Active, Inactive}

    [Required]
    public string IssuerId { get; init; } = string.Empty;

    [Required]
    public string PublicKey { get; init; } = string.Empty;

    [Required]
    public SetStatus Status = SetStatus.Active;

    public TrustRegistry(string issuerId, string publicKey, SetStatus status)
    {
        IssuerId = issuerId;
        PublicKey = publicKey;
        Status = status;
    }
}
