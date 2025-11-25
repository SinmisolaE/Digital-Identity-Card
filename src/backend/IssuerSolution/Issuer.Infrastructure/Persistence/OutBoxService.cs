using System;
using System.Text.Json;
using Hangfire;
using Issuer.Core.Events;
using Issuer.Core.Interfaces.AuthService;
using Issuer.Core.Interfaces.Infrastructure;
using Issuer.Infrastructure.Model;

namespace Issuer.Infrastructure;

public class OutBoxService : IOutBoxService
{
    private readonly AppDbContext _context;

    public OutBoxService(AppDbContext context)
    {
        _context = context;
    }

    // Save event to event box db and hangfire carries out the event
    public async Task SaveEventAsync(object domainEvent)
    {
        var OutBoxMessage = new OutBoxMessage(
            domainEvent.GetType().Name,
            JsonSerializer.Serialize(domainEvent),
            DateTime.UtcNow
        );

        await _context.outBoxMessages.AddAsync(OutBoxMessage);

        BackgroundJob.Enqueue<IEmailService>(x => 
            x.SendPasswordSetEmailAsync(
                ((UserCreatedEvent)domainEvent).Email,
                ((UserCreatedEvent)domainEvent).ResetPasswordToken
            )
        );
        
    }
}
