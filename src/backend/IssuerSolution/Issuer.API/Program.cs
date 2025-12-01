using Hangfire;
using Hangfire.PostgreSql;
using Issuer.Core.Interfaces;
using Issuer.Core.Interfaces.AuthService;
using Issuer.Core.Interfaces.Infrastructure;
using Issuer.Core.Service;
using Issuer.Core.Service.auth;
using Issuer.Core.Service.UserManagement;
using Issuer.Infrastructure;
using Issuer.Infrastructure.Data;
using Issuer.Infrastructure.Email;
using Issuer.Infrastructure.Model;
using Issuer.Infrastructure.Persistence;
using Issuer.Infrastructure.Repository;
using Issuer.Infrastructure.Security;
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

builder.Services.AddScoped<IOutBoxService, OutBoxService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenProvider, UserTokenProvider>();
builder.Services.AddScoped<IPasswordHash, PasswordHash>();

builder.Services.AddScoped<IOutBoxProcessorJob, OutBoxProcessorJob>();

//auth
builder.Services.AddScoped<IAuthService, AuthService>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Connection String: {connectionString}");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Issuer.Infrastructure"))
);

builder.Services.AddHttpClient();

//add registry db
builder.Services.AddHttpClient<ITrustRegistryClient, TrustRegistryClient>();

// add hang fire to handle background processes
builder.Services.AddHangfire(config => 
    config.UsePostgreSqlStorage(options => 
        options.UseNpgsqlConnection(connectionString)
    )
);
builder.Services.AddHangfireServer();



var app = builder.Build();

using var scope = app.Services.CreateScope();
var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<IOutBoxProcessorJob>(
    "process-outbox-messages",
    job => job.ProcessOutBoxMessageAsync(),
    "*/4 * * * *"
);

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

app.MapHealthChecks("/health");

app.Run();
