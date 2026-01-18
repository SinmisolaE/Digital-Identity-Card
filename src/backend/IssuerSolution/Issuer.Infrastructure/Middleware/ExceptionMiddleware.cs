using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Issuer.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionMiddleware> _logger;                                                                                                                                                                                                    

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        } catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, "an error occurred oo");
        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            httpContext.Response.StatusCode,
            Message = "An internal server error occured, please try again later"
        };

        var json = JsonSerializer.Serialize(response);

        return httpContext.Response.WriteAsync(json);
    }
}
