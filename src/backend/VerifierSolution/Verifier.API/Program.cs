using Serilog;
using Verifier.Core.Interfaces;
using Verifier.Core.Services;
using Verifier.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


Log.Logger = new LoggerConfiguration()
.WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();


//HttpClient to request from TrustRegistry
builder.Services.AddHttpClient();

builder.Services.AddHttpClient<ITrustRegistryClient, TrustRegistryClient>(client =>
{
    //client.BaseAddress = new Uri("http://localhost:5051/"); // Note trailing slash
    //client.DefaultRequestHeaders.Add("Accept", "application/json");
}
);

builder.Services.AddScoped<IVerifierService, VerifierService>();

builder.Services.AddScoped<IJwtVerifier, JwtVerifier>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
