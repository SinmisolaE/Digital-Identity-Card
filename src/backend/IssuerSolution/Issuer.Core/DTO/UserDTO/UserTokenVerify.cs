using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Core.DTO.UserDTO;

public class UserTokenVerify
{
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    public string Token {get; set;} = string.Empty;

}
