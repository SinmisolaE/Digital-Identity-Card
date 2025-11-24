using System;

namespace Issuer.Core.Events;

public class UserCreatedEvent
{
    public UserCreatedEvent(int userId, string email, string resetPasswordToken)
    {
        UserId = userId;
        Email = email;
        ResetPasswordToken = resetPasswordToken;
    }

    public int UserId {get; set;}
    public string Email {get; set;}
    public string ResetPasswordToken {get; set;}
}
