using System;
using Microsoft.EntityFrameworkCore;
using TrustRegistryService.Core.Entity;

namespace TrustRegistryService.Infrastructure.Data;

public class TRDbContext : DbContext
{
    public TRDbContext(DbContextOptions<TRDbContext> options) : base(options) { }

    public DbSet<TrustRegistry> TrustRegistries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrustRegistry>()
            .HasIndex(u => u.IssuerId)
            .IsUnique();
    }

}
