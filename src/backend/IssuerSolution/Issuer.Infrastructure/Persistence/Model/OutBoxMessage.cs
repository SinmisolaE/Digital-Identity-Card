using System;
using System.ComponentModel.DataAnnotations;

namespace Issuer.Infrastructure.Model;

// Outbox pattern: entity to handle data and event change together
public class OutBoxMessage
{
    public OutBoxMessage(string eventType, string eventData, DateTime createdAt)
    {
        EventType = eventType;
        EventData = eventData;
        CreatedAt = createdAt;
    }

    [Key]
    public int Id {get; set;}
    public string EventType {get; set;}
    public string EventData {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? ProcessedAt {get; set;}
    public string Error {get; set;}
}
