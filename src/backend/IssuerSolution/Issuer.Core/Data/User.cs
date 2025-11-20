using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Issuer.Core.Data;

public class User
{

    public enum SetStatus {ACTIVE, PENDING, INACTIVE};
    public enum SetRole {ADMIN, ISSUER};

    [Key]
    [Required]
    public int Id {get; private set;}

    [Required]
    public string Email {get; private set;} = string.Empty;

    [Required]
    public SetRole Role {get; private set;}

    [Required]
    public string Hashed_Password {get; private set;} = string.Empty;

    [Required]
    public SetStatus Status {get; private set;}

    public string Reset_password_token {get; private set;} = string.Empty;

    public DateOnly TokenExpiry {get; private set;}

    public DateOnly Creaded_At {get; private set;} = DateOnly.MinValue;

    

}
