using System;
using Issuer.Core.Interfaces.Infrastructure;
using Issuer.Infrastructure.Model;

namespace Issuer.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveEntitiesAsync()
    {
        await _context.SaveChangesAsync();
        return true;
    }
}
