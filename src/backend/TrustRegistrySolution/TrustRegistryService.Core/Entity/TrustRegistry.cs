using System;
using System.ComponentModel.DataAnnotations;

namespace TrustRegistryService.Core.Entity;

public class TrustRegistry
{
    public enum SetStatus {Active, Inactive}

    [Required]
    [Key]
    public string IssuerId { get; init; } = string.Empty;

    [Required]
    public string PublicKey { get; init; } = string.Empty;

    [Required]
    public SetStatus Status;

    public TrustRegistry(string issuerId, string publicKey)
    {
        IssuerId = issuerId;
        PublicKey = publicKey;
        Status = SetStatus.Active;
    }
}
