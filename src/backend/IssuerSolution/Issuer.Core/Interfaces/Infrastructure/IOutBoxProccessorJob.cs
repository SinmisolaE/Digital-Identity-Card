using System;

namespace Issuer.Core.Interfaces.Infrastructure;

public interface IOutBoxProcessorJob
{
    Task ProcessOutBoxMessageAsync();
}
