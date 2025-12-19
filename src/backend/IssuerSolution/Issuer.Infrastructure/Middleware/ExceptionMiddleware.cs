using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Issuer.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
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
