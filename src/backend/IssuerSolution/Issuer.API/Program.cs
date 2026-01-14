using System.Net;
using System.Text;
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
using Issuer.Infrastructure.Middleware;
using Issuer.Infrastructure.Model;
using Issuer.Infrastructure.Persistence;
using Issuer.Infrastructure.Repository;
using Issuer.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt")
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // ← For Minimal APIs

builder.Services.AddHealthChecks();



builder.Services.AddSwaggerGen(options =>
{
    // add jwt token to swagger

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Jwt Authorization",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    };
    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);


    // set up security requirement
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            []
        }
    };

    options.AddSecurityRequirement(securityRequirement);
}
);


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

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))

            
        };
    });

builder.Services.AddAuthorization();


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
app.UseStaticFiles();

using var scope = app.Services.CreateScope();
var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<IOutBoxProcessorJob>(
    "process-outbox-messages",
    job => job.ProcessOutBoxMessageAsync(),
    "*/4 * * * *"
);

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHangfireDashboard();
}

//app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
