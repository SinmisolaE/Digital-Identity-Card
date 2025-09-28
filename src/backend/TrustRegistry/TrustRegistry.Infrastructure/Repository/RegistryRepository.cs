using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrustRegistryService.Core.Entity;
using TrustRegistryService.Core.Interfaces;
using TrustRegistryService.Infrastructure.Data;

namespace TrustRegistryService.Infrastructure.Repository;

public class RegistryRepository : IRegistryRepository
{
    private readonly TRDbContext _context;
    private readonly ILogger<RegistryRepository> _logger;

    public RegistryRepository(ILogger<RegistryRepository> logger, TRDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<TrustRegistry?> GetRegistryByIssuer(string issuer)
    {
        return await _context.TrustRegistries.Where(a => a.IssuerId == issuer).FirstOrDefaultAsync();
    }

}
