using System;
using System.Text.Json;
using Hangfire;
using Issuer.Core.Data;
using Issuer.Core.Events;
using Issuer.Core.Interfaces.AuthService;
using Issuer.Core.Interfaces.Infrastructure;
using Issuer.Infrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Issuer.Infrastructure.Persistence;

public class OutBoxProcessorJob : IOutBoxProcessorJob
{
    private readonly IEmailService _emailService;
    private readonly ILogger<OutBoxProcessorJob> _logger;
    private readonly AppDbContext _context;

    public OutBoxProcessorJob(IEmailService emailService, ILogger<OutBoxProcessorJob> logger, AppDbContext context)
    {
        _emailService = emailService;
        _logger = logger;
        _context = context;
    }


    [AutomaticRetry(Attempts = 5, DelaysInSeconds = new [] {30, 60, 120, 300, 600})]
    public async Task ProcessOutBoxMessageAsync()
    {
        _logger.LogInformation("Starting outbox message processing...");

        var pending_messages = await _context.OutBoxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync();

        foreach (var message in pending_messages)
        {
            try
            {
                _logger.LogInformation($"Processing outbox message {message.Id} of type {message.EventType}");

                switch(message.EventType)
                {
                    case "UserCreatedEvent":
                        var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message.EventData);
                        await _emailService.SendPasswordSetEmailAsync(userEvent.Email, userEvent.ResetPasswordToken);
                        break;

                }

                message.ProcessedAt = DateTime.UtcNow;

            } catch (Exception ex)
            {
                message.Error = ex.Message;
                _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                
                // Don't rethrow - let HangFire handle retries for the entire batch
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Completed processing outbox messages");
    }
}
