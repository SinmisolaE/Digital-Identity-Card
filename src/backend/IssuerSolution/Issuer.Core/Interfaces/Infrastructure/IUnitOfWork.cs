using System;

namespace Issuer.Core.Interfaces.Infrastructure;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync();

}
