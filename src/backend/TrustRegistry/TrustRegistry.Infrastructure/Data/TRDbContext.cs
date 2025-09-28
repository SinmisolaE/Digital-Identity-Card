using System;
using Microsoft.EntityFrameworkCore;
using TrustRegistryService.Core.Entity;

namespace TrustRegistryService.Infrastructure.Data;

public class TRDbContext : DbContext
{
    public TRDbContext(DbContextOptions<TRDbContext> options) : base(options) { }
    
    public DbSet<TrustRegistry> TrustRegistries { get; set;}

}
