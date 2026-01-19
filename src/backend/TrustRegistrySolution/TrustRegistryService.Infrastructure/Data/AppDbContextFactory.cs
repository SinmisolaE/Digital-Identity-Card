using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TrustRegistryService.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<TRDbContext>
{
    public TRDbContext CreateDbContext(string[] args)
    {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "TrustRegistryService.API")) // Adjust the path as needed
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                //.AddJsonFile("secrets.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TRDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new TRDbContext(optionsBuilder.Options);
    }
}
