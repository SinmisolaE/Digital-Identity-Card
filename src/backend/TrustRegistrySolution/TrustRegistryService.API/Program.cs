using Microsoft.EntityFrameworkCore;
using Serilog;
using TrustRegistryService.Core.Interfaces;
using TrustRegistryService.Core.Services;
using TrustRegistryService.Infrastructure.Data;
using TrustRegistryService.Infrastructure.Middleware;
using TrustRegistryService.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt")
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

//

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TRDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
    b => b.MigrationsAssembly("TrustRegistryService.Infrastructure"))
);


//Inject services
builder.Services.AddScoped<IRegistryRepository, RegistryRepository>();
builder.Services.AddScoped<IRegistryService, RegistryService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
