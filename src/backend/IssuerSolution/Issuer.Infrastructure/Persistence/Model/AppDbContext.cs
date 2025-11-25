using System;
using Issuer.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Issuer.Infrastructure.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users {get; set;}
    public DbSet<OutBoxMessage> OutBoxMessages {get; set;}



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<OutBoxMessage>(entity =>
        {
            entity.HasKey(x => x.Id);

        });
    }

}
