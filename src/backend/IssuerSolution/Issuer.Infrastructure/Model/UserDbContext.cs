using System;
using Issuer.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Issuer.Infrastructure.Model;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();
    }

}
