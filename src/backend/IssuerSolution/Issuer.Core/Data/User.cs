using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Issuer.Core.Data;

public class User
{
    public User(string email, SetRole role)
    {
        Email = email;
        Role = role;
    }

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

    public string ResetPasswordToken {get; private set;} = string.Empty;

    public DateTime TokenExpiry {get; private set;}

    public DateTime Creaded_At {get; private set;} = DateTime.UtcNow;

    public void AssignToken(string token)
    {
        this.ResetPasswordToken = token;
        this.TokenExpiry = DateTime.UtcNow.AddHours(72);
    }

}
