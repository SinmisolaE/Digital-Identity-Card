using Hangfire;
using Hangfire.PostgreSql;
using Issuer.Core.Interfaces;
using Issuer.Core.Service;
using Issuer.Infrastructure;
using Issuer.Infrastructure.Data;
using Issuer.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt")
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // ← For Minimal APIs

builder.Services.AddHealthChecks();


//builder.Logging.ClearProviders();

builder.Services.AddSwaggerGen();


//Add serilog to system
builder.Logging.AddSerilog();


builder.Services.AddScoped<IIssuerService, IssuerService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddSingleton<IRsaKeyService, RsaKeyService>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Issuer.Infrastructure"))
);

builder.Services.AddHttpClient();

//add registry db
builder.Services.AddHttpClient<ITrustRegistryClient, TrustRegistryClient>();


// add hang fire to handle background processes
builder.Services.AddHangfire(config => 
    config.UsePostgreSqlStorage(connectionString)
);
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHangfireDashboard();
}

//app.UseHttpsRedirection(); 


app.UseAuthorization();

app.MapControllers();

app.MapGet("/test", () => "Test working!");

app.MapHealthChecks("/health");

app.Run();
