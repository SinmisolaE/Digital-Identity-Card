var builder = WebApplication.CreateBuilder(args);

// add yarp as the gateway to services
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

// enable cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseCors("CorsPolicy");
}

app.UseHttpsRedirection();
app.UseAuthorization();


app.MapReverseProxy();

app.MapHealthChecks("/health");

//app.MapControllers();

app.Run();
