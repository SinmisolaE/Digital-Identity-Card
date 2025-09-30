using System;
using System.ComponentModel.DataAnnotations;

namespace Verifier.Core.DTO;

public class JwtDTO
{
    [Required]
    string Jwt { get; init; } = string.Empty;
}
