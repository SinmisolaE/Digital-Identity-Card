using System;

namespace Issuer.Core.Interfaces.Infrastructure;

public interface IOutBoxService
{
    Task SaveEventAsync(object domainEvent);
}
