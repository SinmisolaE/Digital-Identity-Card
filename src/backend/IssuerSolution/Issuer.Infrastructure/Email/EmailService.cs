using System;
using MailKit.Net.Smtp;
using Issuer.Core.Interfaces.AuthService;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Hangfire;
using Microsoft.Extensions.Logging;
using MailKit.Security;

namespace Issuer.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    private readonly string _sender;
    private readonly string _password;
    private readonly string _smtpHost;
    private readonly int _port;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
        _configuration = configuration;
        _sender = _configuration["Email:Sender"];
        _password = _configuration["Email:password"];
        _smtpHost = _configuration["Email:smtpHost"];
        _port = _configuration.GetValue<int>("Email:smtpPort");
        
    }


    // Sends token to issuer email upon profiling
    // Add automatic retries using hangfire if service goes down
    public async Task<bool> SendPasswordSetEmailAsync(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token)) return false;

        // dummy message
        var body = $@"
        Hello,

        You requested to reset your password. Use the following verification code:

        Verification Code: {token}

        Enter this code on the password reset page to continue.

        If you didn't request this reset, please ignore this email.

        Thanks,
        The App Team";

        // Generate message

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("", _sender));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Setup Required";
        message.Body = new TextPart("plain")
        {
            
            Text = body
        };

        _logger.LogInformation($"Host: {_smtpHost}");

        // Set email connection
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpHost, _port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_sender, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }



        
        return true;
    }
}
