using System;
using Issuer.Core.Interfaces.AuthService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Issuer.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _fromEmail;
    private readonly string _password;
    private readonly string _smtpHost;
    private readonly int _port;

    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _fromEmail = _configuration["Email:Sender"];
        _password = _configuration["Email:password"];
        _smtpHost = _configuration["Email:smtpHost"];
        _port = _configuration.GetValue<int>("Email:smtpPort");
        _logger = logger;
    }

    public async Task<bool> SendPasswordSetEmailAsync(string email, string token)
    {
        var subject = "Password set required";

        var message = new MimeMessage();
        var body = $"Hello {email}";


        message.From.Add(new MailboxAddress("", _fromEmail));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };


        try
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpHost, _port, true);
                await client.AuthenticateAsync(_fromEmail, _password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        } catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return false;
        }
    }
}
