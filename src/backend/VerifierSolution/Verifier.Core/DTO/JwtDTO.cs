using System;
using System.ComponentModel.DataAnnotations;

namespace Verifier.Core.DTO;

public class JwtDTO
{
    [Required]
    public string Jwt { get; init; } = string.Empty;

    public string nonce { get; init; } = string.Empty;


}
