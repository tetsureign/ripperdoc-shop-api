using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RipperdocShop.Api.Infrastructure.Errors;

public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentOutOfRangeException => (StatusCodes.Status400BadRequest, "Invalid request"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Invalid operation"),
            _ => (StatusCodes.Status500InternalServerError, "Unexpected error")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception while processing {Path}", httpContext.Request.Path);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await JsonSerializer.SerializeAsync(httpContext.Response.Body, problem, cancellationToken: cancellationToken);

        return true;
    }
}
